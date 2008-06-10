#!/usr/bin/env ruby  

# graphic libs
require 'gtk2'
require 'cairo'
require 'pango'
require 'rsvg2'


# internal includes
require 'tcal/event'
require 'tcal/layer'
require 'tcal/cairo_aux'



module TCal

    class Calendar < Gtk::DrawingArea

        # defualt list of background colors (RGBA)
        # FIXME: ugly
        @@colors_bg = [
            [1.0,0.0,0.0,0.7],
            [0.0,1.0,0.0,0.7],
            [0.0,0.0,1.0,0.7],
            [0.0,1.0,0.5,0.7],
            [0.5,0.4,1.0,0.7],
            [1.0,1.0,0.0,0.7],
            [0.5,1.0,0.0,0.7],
            [0.7,0.1,0.1,0.7],
            [0.1,0.7,0.1,0.7],
        ]

        # defualt list of border colors (RGBA)
        # FIXME: ugly
        @@colors_border = [
            [0.8,0.0,0.0,0.9],
            [0.0,0.8,0.0,0.9],
            [0.0,0.0,0.8,0.9],
            [0.0,1.0,0.3,0.9],
            [0.5,0.4,1.0,0.9],
            [1.0,1.0,0.0,0.9],
            [0.5,1.0,0.0,0.9],
            [0.7,0.1,0.1,0.9],
            [0.1,0.7,0.1,0.9],


        ]


        # send a hash of parameters, please
        #
        # list of options:
        #
        # [start_day] the first day of the weed. default: sunday. 
        # the days are numbered 1=>sun,2=>mon...7=>sat
        #
        # [end_day] the last day of the week. default: thursday
        # the days are numbered 1=>sun,2=>mon...7=>sat
        #
        # [start_hour] the first hour of the day. default 8:30
        # the numbers represent hours, so 8.5 is 8 and a half hours, or 8:30
        #
        # [end_hour] the first hour of the day. default 20:30
        # the numbers represent hours, so 8.5 is 8 and a half hours, or 8:30
        #
        # [jump_hour] the increments by which hours are drawn and written
        #
        # [major_hour] how many increments consist of a full unit (and get a bold line)
        #
        # [major_mod] which increment gets the bold line ( mod as in the operator % )
        #
        # [line_width] the width if the drawn line. default 0.5, which is a fuzzy 2 pixels after anti-aliasing 
        #
        # [logo] an SVG to use as the background logo
        #
        #
        def initialize(params={})
            super()

            # initialize the list of events
            @events = []


            @start_day = params[:start_day] || 1
            @end_day = params[:end_day] || 5

            # compute the number of days
            if @end_day > @start_day
                @days = @end_day - @start_day + 1
            else
                @days = @end_day + 8 - @start_day
            end


            @start_hour = params[:start_hour] || 8.5
            @end_hour = params[:end_hour] || 20.5
            @major_hour = params[:major_hour] || 2 #once in how many jumps do we use a full line
            @major_mod = params[:major_mod] || 1 # which part of the hour do we mark as major
            @jump_hour = params[:jump_hour] || 0.5

            #cumpute how many parts of a day we have
            @hour_segments = ((@end_hour - @start_hour) / @jump_hour  ).to_i 


            @computed_layers=false

            @line_width = params[:line_width] || 0.5


            #logo
            @logo = params[:logo]
            if not @logo.nil?
                @logo_handle =  RSVG::Handle.new_from_file(@logo)
                @logo_dim = @logo_handle.dimensions
            end


            self.signal_connect("expose-event") do
                self.draw_sched
                true
            end

            self.add_events(Gdk::Event::ALL_EVENTS_MASK)
            self.has_tooltip = true

            self.signal_connect("query-tooltip") do |calendar, x, y, v, z|
              day = day_at_x(x)
              hour = hour_at_y(y)
              ratio = (x % step_width) / step_width.to_f
              if day and hour and ratio
                event = @events.find {|ev| ev.catches_click?(day, hour, ratio) }
                if event
                  calendar.tooltip_markup = event.markup
                end
              end
              false
            end


            @click_handlers = []

            self.signal_connect("button-press-event") do |calendar, e|
              day = day_at_x(e.x)
              hour = hour_at_y(e.y)
              ratio = (e.x % step_width) / step_width.to_f
              event = @events.find { |ev| ev.catches_click?(day, hour, ratio) }
              @click_handlers.each do |handler|
                handler.call({
                  :day => day,
                  :hour => hour,
                  :event => event,
                })
              end
            end
        end

        def add_click_handler(&handler)
          @click_handlers << handler
        end

        def day_at_x(x)
          day = @days - (x / step_width).to_i
          return nil if day < 0
          day
        end

        def hour_at_y(y)
          hour = ((y / step_height).to_i - 1) * @jump_hour + @start_hour
          return nil if hour < @start_hour
          hour
        end

        # returns a new cairo context for the drawing area
        def get_cairo
            self.window.create_cairo_context
        end




        # itterate over all the items and set their layers and ratios so that they are draws 
        # with correct positioning for multiple items during the same times
        #
        # general explanation:
        # every we coose an event x and add it to [selected], under the condition
        # that x was not added to [selected] yet.
        # every itteration we check the list of events that are not in [selected]
        # and see if any collide with an event in selected. if so we add them to [selected]
        # when no collisions remain, we devide the events in [selected] into groups so that
        # no 2 events in the same group collide. then we assing each group a layer and the
        # ratio is the inverse of the number of groups
        def compute_layers
            @computed_layers=true
            remaining = @events #all events have not been assigned to a collision group

            #every itteration we compute a collision group, split it into layers, and assign ratios
            while not remaining.empty? do
                # add a random event
                selected = [remaining[0]]
                cont = true
                # cont means we added a new event to selected
                while(cont)
                    cont=false
                    # get a new list
                    old_selected = selected
                    selected = []
                    old_selected.each do |s|

                        # avoid duplicates
                        selected << s unless selected.include?(s)
                        remaining.each do |r|

                            # there's a collision, add r to selected
                            if s.collides_with?(r)

                                # avoid duplicates
                                selected << r  unless selected.include?(r)
                                cont=true # a new event has been added to s. continue
                            end
                        end
                    end

                    # recompute the remaining
                    rejected = []
                    remaining.each do |r|
                        rejected << r unless selected.include?(r)
                    end
                    remaining = rejected

                end

                # devide into layers
                layers = []
                layer_num=0
                selected.each do |s|
                    cur_layer=0
                    begin
                        while layers[cur_layer].collides_with?(s) do
                            cur_layer += 1
                        end

                        # found a layer that's ok
                        layers[cur_layer] << s
                        s.layer=cur_layer

                    rescue

                        #collides with all layers. add a new layer
                        layers[cur_layer] = Layer.new
                        layers[cur_layer] << s
                        s.layer=cur_layer
                        layer_num += 1

                    end
                end

                #compute the ratio
                selected.each do |s|
                    s.ratio = 1.0/layer_num
                end

            end
        end


        # add a new event to the sched
        def add_event(text,day,hour,length,color,group)
            @events << Event.new(text,day,hour,length,color,1,0,group)
            @computed_layers=false
        end

        # remove all events
        def clear_events
            @events = [] 
            @computed_layers=false
        end

        def redraw
            self.window.invalidate(Gdk::Rectangle.new(0, 0, width, height),false)
        end


        def draw_item(cairo,item)

            # set internal line width for this function
            line_width=3.0
            half_line_width=line_width/2 #optimizaton

            # get size and compute ratios
            hour_steps = (item.hour - @start_hour)/(@jump_hour) 
            length_steps = item.length/@jump_hour
            day_steps = (item.day + 7 - @start_day) % 7 
            layer_width = (step_width*item.ratio).to_i
            item_length = (step_height*length_steps).to_i

            # translate to where we want to draw
            cairo.translate(step_width*(@days-day_steps-1),(hour_steps+1)*step_height)

            # load color
            clr = @@colors_border[item.color_id]
            cairo.set_source_rgba(clr[0],clr[1],clr[2],clr[3])

            # draw path
            cairo.rounded_rectangle(half_line_width+layer_width*item.layer, 
                                    half_line_width,
                                    layer_width*(item.layer+1)-half_line_width, # move to the next layer
                                    item_length-half_line_width)
            cairo.set_line_width(line_width)

            # stroke path
            cairo.stroke_preserve

            # load bg color
            clr = @@colors_bg[item.color_id]
            cairo.set_source_rgba(clr[0],clr[1],clr[2],clr[3])

            # fill path
            cairo.fill

            # create clipping region for text
            cairo.rectangle(0,0,step_width,item_length-line_width-1)


            cairo.clip

            # set text color
            cairo.set_source_rgba(0,0,0,1)

            # set text position
            cairo.move_to(3+(layer_width*item.layer),3)

            #draw text
            cairo.pango_render_text((layer_width-6),"Sans 8",item.markup)

            #reset damage
            cairo.reset_clip
            cairo.identity_matrix
        end

        # turn fraction time ino a formated time string, aka 12.25 => "12:30"
        def to_time(time_frac)
            hours = time_frac.floor
            frac = time_frac-hours
            mins = (frac*60).floor
            return sprintf("%d:%02d",hours,mins)
        end

        def width
          self.window.size[0]
        end

        def height
          self.window.size[1]
        end

        def step_width
          (width + @line_width) / (@days + 1)
        end

        def step_height
          (height + @line_width) / (@hour_segments+1)
        end

        def get_bg_image
            if @bg_image.nil? or width != @bg_image_width or height != @bg_image_height

                # initialize for drawing
                @bg_image_width=width
                @bg_image_height=height

                #this will be the bg image at the end
                surf = Cairo::ImageSurface.new(Cairo::FORMAT_ARGB32, width,height)
                cairo = Cairo::Context.new(surf)

                # get cairo and set constants
                cairo.set_line_join(Cairo::LINE_JOIN_ROUND)
                cairo.set_line_cap(Cairo::LINE_CAP_ROUND)
                cairo.set_line_width(@line_width)

                # gray BG for border
                cairo.rectangle(0,0,width,height)
                lin = cairo.linear_gradient(0,height,0,0, :reflect,[1.0,gray],[1.0 - 1.0/@hour_segments,lgray], [0.0 ,llgray])
                cairo.set_source(lin)
                cairo.fill()

                # white for bg
                cairo.rectangle(0,step_height,width-step_width,height)
                cairo.set_source_rgb(1.0,1.0,1.0)
                cairo.fill()


                #logo
                if not @logo.nil?

                    # rsvg
                    cairo.translate 0,step_height
                    cairo.render_rsvg_centered(width - step_width,height - step_height, @logo_handle)
                    cairo.identity_matrix

                    #fade_logo
                    cairo.set_source_rgba(1.0,1.0,1.0,0.8)
                    cairo.rectangle(0,step_height,width-step_width,height)
                    cairo.fill()
                end


                # compute optimal font size
                font_size = (step_height * 0.60).to_i
                font_size = 8 if font_size < 8 # you cant read less than 8
                font_size = 12 if font_size > 12 # over 12 it becomes oversized and ugly

                # set grid gradient
                lin = cairo.linear_gradient(0,height,0,0, :reflect,[0.0,oblack],[0.3,ooblack])
                cairo.set_source(lin)

                # itterate to draw
                @days.downto 1 do |i|

                    # draw line
                    cairo.move_to step_width*i, 0
                    cairo.rel_line_to 0, height
                    cairo.stroke
                    cairo.move_to step_width*(i-1)+3, (0.2 * step_height).to_i - @line_width

                    # render day name
                    cairo.pango_render_text((step_width)-6,"Sans #{font_size}","<tt>#{day_names[(@days-i-1+@start_day)%7]}</tt>")

                end



                # itterate to draw
                @hour_segments.downto 1 do |i|


                    # compute if this is a major line, and if so make it full width
                    # otherwise make it narrow
                    if (i % @major_hour == @major_mod)
                        cairo.set_line_width(@line_width)
                    else
                        cairo.set_line_width(@line_width * 0.20)
                    end

                    # draw line
                    cairo.move_to 0, (step_height*i).to_i
                    cairo.rel_line_to width, 0
                    cairo.stroke
                    cairo.move_to width-step_width+3, (step_height*(i+0.2)).to_i - @line_width

                    # render hour
                    cairo.pango_render_text((step_width)-6,
                                         "Sans #{font_size}",
                                          "<tt>#{to_time(@start_hour+(@jump_hour*(i-1)))}</tt>")

                end
                @bg_image =  Cairo::SurfacePattern.new(surf)       
            end

            # what we wanted
            return @bg_image
        end


        # main graphic function
        # this functions draws the grid lines, the backgrounds, the logo, the hours, 
        # and the itterates to draw the events
        # FIXME: refactor to several functions
        def draw_sched

            cairo = self.get_cairo

            # draw the grid background
            bg_grid = get_bg_image
            cairo.set_source(bg_grid)
            cairo.rectangle(0,0,width,height)
            cairo.fill

            compute_layers unless @computed_layers # make sure we have layers computed

            # render events
            #
#            Profiler__::start_profile
            @events.each do |i| 
                draw_item(cairo,i)
            end
#            Profiler__::stop_profile
#            Profiler__::print_profile($stderr)
        end


        # here is a load of constants that needs to be moved elsewhere

        def white 
            [1.0,1.0,1.0, 1]
        end
        def owhite
            [1.0,0.95,0.9, 1]
        end

        def llgray
            [0.98,0.98,0.98, 1]
        end

        def lgray
            [0.9,0.9,0.9, 1]
        end

        def gray
            [0.8,0.8,0.8, 1]
        end

        def black
            [0,0,0,1]
        end

        def oblack
            [0.3,0.3,0.3,1]
        end


        def ooblack
            [0.5,0.5,0.5,1]
        end

        def day_names
            ["ראשון","שני","שלישי","רביעי","חמישי","שישי","שבת"]
        end


    end
end
