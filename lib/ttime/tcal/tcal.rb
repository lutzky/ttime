#!/usr/bin/env ruby

# graphic libs
require 'gtk2'
require 'cairo'
require 'pango'
require 'rsvg2'


# internal includes
require 'ttime/tcal/event'
require 'ttime/tcal/layer'
require 'ttime/tcal/cairo_aux'

LEFT_BUTTON = 1
RIGHT_BUTTON = 3

module TCal
    class Calendar < Gtk::DrawingArea
      DAY_NAME_FONT = "Sans Bold"
      HOUR_FONT = "Sans"

      MAX_START_HOUR = 8.5
      MIN_END_HOUR = 16.5
      MAX_START_DAY = 1
      MIN_END_DAY = 5

      LLGRAY = [0.98, 0.98, 0.98, 1]
      LGRAY = [0.9,0.9,0.9, 1]
      GRAY = [0.8,0.8,0.8, 1]
      OBLACK = [0.3,0.3,0.3, 1]

      # Default color list. All colors are represented in RGBA.
      @@colors = [
          [1.00,0.61,0.61,0.70],
          [0.62,0.61,1.00,0.70],
          [0.61,1.00,0.62,0.70],
          [1.00,1.00,0.61,0.70],
          [1.00,0.61,0.97,0.70],
          [0.61,0.84,1.00,0.70],
          [1.00,0.81,0.61,0.70],
      ]

      def modify_color(color, params = {})
        factor = params[:brightness] || 1.0
        alpha = params[:alpha] || color[3]
        return [
          color[0] * factor,
          color[1] * factor,
          color[2] * factor,
          alpha
        ]
      end

      def get_color(type, index)
        color = @@colors[index % @@colors.length]
        case type
        when :tutorial
          return modify_color(color, :brightness => 0.8)
        when :border
          return modify_color(color, :brightness => 0.7, :alpha => 0.9)
        else
          return color
        end
      end

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


            @start_day = params[:start_day] || MAX_START_DAY
            @end_day = params[:end_day] || MIN_END_DAY

            @start_hour = params[:start_hour] || MAX_START_HOUR
            @end_hour = params[:end_hour] || MIN_END_HOUR
            @major_hour = params[:major_hour] || 2 #once in how many jumps do we use a full line
            @major_mod = params[:major_mod] || 1 # which part of the hour do we mark as major
            @jump_hour = params[:jump_hour] || 0.5

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

            @click_handlers = []

            self.signal_connect("button-press-event") do |calendar, e|
              day = day_at_x(e.x)
              hour = hour_at_y(e.y)
              ratio = (e.x % step_width) / step_width.to_f
              event = @events.find { |ev| ev.catches_click?(day, hour, ratio) }
              @click_handlers.each do |handler|
                handler.call({
                  :day => day,
                  :hour => TTime::Logic::Hour::float_to_military(hour),
                  :data => (event and event.data),
                  :gdk_event => e,
                })
              end
            end
        end

        def add_rightclick_handler(&handler)
          @click_handlers << Proc.new do |params|
            if params[:gdk_event].button == RIGHT_BUTTON
              handler.call params
            end
          end
        end

        def add_click_handler(&handler)
          @click_handlers << handler
        end

        def day_at_x(x)
          day = days - (x / step_width).to_i
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
        def add_event(text,day,hour,length,color,data = nil,type = nil)
            @events << Event.new(text,day,hour,length,color,1,0,data,type)
            @computed_layers=false
        end

        def reject_events! &blk
          @events.reject! do |ev|
            blk.call ev.data
          end
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
            cairo.translate(step_width*(days-day_steps-1),(hour_steps+1)*step_height)

            # load color
            clr = get_color(:border, item.color_id)
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
            clr = get_color(item.type, item.color_id)
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
          (width + @line_width) / (days + 1)
        end

        def step_height
          (height + @line_width) / (hour_segments+1)
        end

        def hour_segments
            ((@end_hour - @start_hour) / @jump_hour).to_i
        end

        def days
          @end_day - @start_day + 1
        end

        def schedule_boundaries
          [ @start_day, @end_day, @start_hour, @end_hour ]
        end

        def get_bg_image
          unless @bg_image_schedule_boundaries != schedule_boundaries or
            @bg_image.nil? or
            width != @bg_image_width or height != @bg_image_height

            # Previously calculated @bg_image is OK, return that
            return @bg_image
          end

          @bg_image_width=width
          @bg_image_height=height

          @bg_image_schedule_boundaries = schedule_boundaries

          # @bg_image itself will be generated from surf
          surf = Cairo::ImageSurface.new(Cairo::FORMAT_ARGB32, width,height)
          cairo = Cairo::Context.new(surf)

          cairo.set_line_join(Cairo::LINE_JOIN_ROUND)
          cairo.set_line_cap(Cairo::LINE_CAP_ROUND)
          cairo.set_line_width(@line_width)

          # gray BG for border
          cairo.rectangle(0,0,width,height)
          lin = cairo.linear_gradient(0,height,0,0, :reflect,[1.0,GRAY],[1.0 - 1.0/hour_segments,LGRAY], [0.0, LLGRAY])
          cairo.set_source(lin)
          cairo.fill()

          # white for bg
          cairo.rectangle(0,step_height,width-step_width,height)
          cairo.set_source_rgb(1.0,1.0,1.0)
          cairo.fill()

          # Draw logo if selected
          unless @logo.nil?
            cairo.translate 0,step_height
            cairo.render_rsvg_centered(width - step_width,height - step_height, @logo_handle)
            cairo.identity_matrix

            # ...with a nice fade-out effect
            cairo.set_source_rgba(1.0,1.0,1.0,0.8)
            cairo.rectangle(0,step_height,width-step_width,height)
            cairo.fill()
          end

          # Note: Font sizes should not be above 12, they tend to look bad.
          # However, there is no reason for a lower-bound on the font size:
          # if we set one, rows will kludge together and we lose legibility
          # all the same.
          font_size = [ (step_height * 0.60).to_i, 12 ].min

          # set grid gradient
          lin = cairo.linear_gradient(0,height,0,0, :reflect,[0.0,OBLACK],[0.3,OBLACK])
          cairo.set_source(lin)

          # Draw day names and appropriate lines
          days.downto 1 do |i|
            cairo.move_to step_width*i, 0
            cairo.rel_line_to 0, height
            cairo.stroke
            cairo.move_to step_width*(i-1)+3, (0.2 * step_height).to_i - @line_width
            font = "#{DAY_NAME_FONT} #{font_size}"
            cairo.pango_render_text((step_width)-6, font,
                      "#{day_names[(days-i-1+@start_day)%7]}")
          end

          # Draw hour labels and appropriate lines
          hour_segments.downto 1 do |i|
            # compute if this is a major line, and if so make it full width
            # otherwise make it narrow
            if (i % @major_hour == @major_mod)
              cairo.set_line_width(@line_width)
            else
              cairo.set_line_width(@line_width * 0.20)
            end

            cairo.move_to 0, (step_height*i).to_i
            cairo.rel_line_to width, 0
            cairo.stroke
            cairo.move_to width-step_width+3, (step_height*(i+0.1)).to_i - @line_width

            font = "#{HOUR_FONT} #{font_size}"
            hour_text = to_time(@start_hour+(@jump_hour*(i-1)))
            cairo.pango_render_text((step_width)-6, font, hour_text)
          end

          @bg_image = Cairo::SurfacePattern.new(surf)
          return @bg_image
        end

        def draw_sched
          start_hours = @events.collect { |ev| ev.hour }
          start_hours << MAX_START_HOUR
          end_hours = @events.collect { |ev| ev.hour + ev.length }
          end_hours << MIN_END_HOUR
          busy_days = @events.collect { |ev| ev.day }
          busy_days << MAX_START_DAY << MIN_END_DAY

          @start_hour = start_hours.min
          @end_hour = end_hours.max
          @start_day = busy_days.min
          @end_day = busy_days.max

          cairo = self.get_cairo

          # draw the grid background
          bg_grid = get_bg_image
          cairo.set_source(bg_grid)
          cairo.rectangle(0,0,width,height)
          cairo.fill

          compute_layers unless @computed_layers

          @events.each do |i|
            draw_item(cairo,i)
          end
        end

        def day_names
            ["ראשון","שני","שלישי","רביעי","חמישי","שישי","שבת"]
        end
    end
end
