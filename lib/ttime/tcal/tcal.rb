#!/usr/bin/env ruby

require 'set'

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
    # Units for landscape A4. TODO: Not sure why the 4, 2 factors are required
    PDF_Width = 210 * 4
    PDF_Height = 297 * 2

    # Initialize a calendar. Possible parameters (within +params+) are:
    #
    # [<tt>:logo</tt>] An SVG filename to use as the background logo
    def initialize(params={})
      super()

      @events = []

      @cairo = nil
      @bg_image = nil

      @start_day = MAX_START_DAY
      @end_day = MIN_END_DAY

      @start_hour = MAX_START_HOUR
      @end_hour = MIN_END_HOUR

      @computed_layers=false

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
        unless day.nil? or hour.nil?
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
    end

    # Register a click handler. If a displayed event is clicked on,
    # <tt>:data</tt> is the original data sent with this event on add_event.
    def add_click_handler(&handler) #:yields: {:day,:hour,:data,:gdk_event}
      @click_handlers << handler
    end

    # Registers a handler for right-clicks. See add_click_handler.
    def add_rightclick_handler(&handler) #:yields: {:day,:hour,:data,:gdk_event}
      @click_handlers << Proc.new do |params|
        if params[:gdk_event].button == RIGHT_BUTTON
          handler.call params
        end
      end
    end

    # Adds a new event to the schedule. +color+ should be an index of coloring
    # group -- an actual color will be assigned based on this and +type+. If
    # specified, +data+ will be sent to any registered click handlers (see
    # add_click_handler)
    def add_event(text,day,hour,length,color,data = nil,type = nil)
      @events << Event.new(text,day,hour,length,color,1,0,data,type)
      @computed_layers=false
    end

    # Rejects events from the calendar using the given block. The yielded
    # +data+ is whatever you sent as +data+ in add_event.
    def reject_events! &blk # :yields: data
      @events.reject! do |ev|
        blk.call ev.data
      end
      @computed_layers=false
    end

    # Update event text for each event in the calendar. Your block should
    # return a new "text" value for the event object in question, or +nil+ to
    # leave the event unmodified. The yielded +data+ is whatever you sent as
    # +data+ in add_event.
    def update_event_text &blk # :yields: data
      @events.each do |ev|
        retval = blk.call ev.data
        ev.text = retval unless retval.nil?
      end
    end

    # Remove all events from the calendar
    def clear_events
      @events = []
      @computed_layers=false
    end

    # Redraw the schedule
    def redraw
      # Send an invalidation event so the schedule gets redrawn
      self.window.invalidate(Gdk::Rectangle.new(0, 0, width, height),false)
    end

    def output_pdf(filename)
      draw_sched(filename)
    end

    protected

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

    DAY_NAMES = ["ראשון","שני","שלישי","רביעי","חמישי","שישי","שבת"]

    EVENT_COLORS = [
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

    # Get an appropriate color for an event of type +type+, with group index
    # +index+. Try and keep +index+ values in order - colors are chosen
    # cyclically out of EVENT_COLORS.
    def get_color(type, index)
      color = EVENT_COLORS[index % EVENT_COLORS.length]
      case type
      when :tutorial
        return modify_color(color, :brightness => 0.8)
      when :border
        return modify_color(color, :brightness => 0.7, :alpha => 0.9)
      else
        return color
      end
    end

    # Hours are drawn and written by increments of JUMP_HOUR
    JUMP_HOUR = 0.5
    # A full line is used every MAJOR_HOUR hours
    MAJOR_HOUR = 2
    # The MAJOR_MOD part of the hour is marked as "major"
    MAJOR_MOD = 1

    LINE_WIDTH = 0.5

    # The (1-indexed) day at the given x coordinate
    def day_at_x(x)
      day = days - (x / step_width).to_i
      return nil if day < 1
      day
    end

    # The hour at the given y coordinate
    def hour_at_y(y)
      hour = ((y / step_height).to_i - 1) * JUMP_HOUR + @start_hour
      return nil if hour < @start_hour
      hour
    end

    # Creates a Cairo context for the drawing area
    def get_cairo
      @cairo = self.window.create_cairo_context
    end

    # Iterate over all items and set their layers and ratios so that they are
    # drawn with correct positioning for overlapping items.
    #
    # = General explanation
    # While there are remaining events, create an arbitrary +selected+ group
    # of size 1. Grow this group to contain any colliding events until
    # reaching a fixed-point. Then greedily assign layers to each event within
    # the group.
    def compute_layers
      @computed_layers=true

      remaining = @events

      # As long as there are remaining events, build maximal collision groups
      # and assign layers and ratios to events within those groups.
      while not remaining.empty? do
        # Select an arbitrary event
        selected = Set.new([remaining[0]])
        selected_more_events = true

        # Add events colliding with selected events until reaching a
        # fixed-point
        while selected_more_events
          selected_more_events = false
          old_selected = selected
          selected = Set.new

          old_selected.each do |s|
            selected << s
            remaining.each do |r|
              if s.collides_with?(r)
                selected << r
                selected_more_events = true
              end
            end
          end

          # Interesting bug - remaining.reject! does not do what we
          # expect, for some reason. (rejects everything)
          remaining = remaining.reject { |x| selected.include?(x) }
        end

        # Now we take our group selection and divide it up into layers

        layers = []

        selected.each do |s|
          s.layer = nil
          layers.each_with_index do |layer, i|
            unless layer.collides_with?(s)
              layer << s
              s.layer = i
              break
            end
          end

          if s.layer.nil?
            # No layer has been assigned yet, so all layers must collide with
            # s. Create a new one.
            layers << Layer.new([s])
            s.layer = layers.size - 1
          end
        end

        # Now all events within the collision group are divided among
        # layers.size layers, so we give them the appropriate ratio
        ratio = 1.0 / layers.size
        selected.each do |s|
          s.ratio = ratio
        end
      end
    end

    # Draw a TCal::Event in the appropriate position
    def draw_item item
      get_cairo if @cairo.nil?

      # set internal line width for this function
      line_width=3.0
      half_line_width=line_width/2 #optimizaton

      # get size and compute ratios
      hour_steps = (item.hour - @start_hour)/JUMP_HOUR
      length_steps = item.length/JUMP_HOUR
      day_steps = (item.day + 7 - @start_day) % 7
      layer_width = (step_width*item.ratio).to_i
      item_length = (step_height*length_steps).to_i

      # translate to where we want to draw
      @cairo.translate(step_width*(days-day_steps-1),(hour_steps+1)*step_height)

      # load color
      clr = get_color(:border, item.color_id)
      @cairo.set_source_rgba(*clr)

      # draw path
      @cairo.rounded_rectangle(half_line_width+layer_width*item.layer,
                              half_line_width,
                              layer_width*(item.layer+1)-half_line_width, # move to the next layer
                              item_length-half_line_width)
      @cairo.set_line_width(line_width)

      # stroke path
      @cairo.stroke_preserve

      # load bg color
      clr = get_color(item.type, item.color_id)
      @cairo.set_source_rgba(clr[0],clr[1],clr[2],clr[3])

      # fill path
      @cairo.fill

      # create clipping region for text
      @cairo.rectangle(0,0,step_width,item_length-line_width-1)


      @cairo.clip

      # set text color
      @cairo.set_source_rgba(0,0,0,1)

      # set text position
      @cairo.move_to(3+(layer_width*item.layer),3)

      #draw text
      @cairo.pango_render_text((layer_width-6),"Sans 8",item.markup)

      #reset damage
      @cairo.reset_clip
      @cairo.identity_matrix
    end

    # Format fraction time ino a formated time string, e.g. 12.25 => "12:30"
    def to_time(time_frac)
      hours = time_frac.floor
      frac = time_frac-hours
      mins = (frac*60).floor
      return sprintf("%d:%02d",hours,mins)
    end

    def width
      if @use_pdf_measurements
        return PDF_Width
      else
        return self.window.size[0]
      end
    end

    def height
      if @use_pdf_measurements
        return PDF_Height
      else
        self.window.size[1]
      end
    end

    def step_width
      (width + LINE_WIDTH) / (days + 1)
    end

    def step_height
      (height + LINE_WIDTH) / (hour_segments+1)
    end

    def hour_segments
      ((@end_hour - @start_hour) / JUMP_HOUR).to_i
    end

    def days
      @end_day - @start_day + 1
    end

    def schedule_boundaries
      [ @start_day, @end_day, @start_hour, @end_hour ]
    end

    def get_bg_image output_immediate = false
      unless @bg_image.nil? or                                  \
        @bg_image_schedule_boundaries != schedule_boundaries or \
        width != @bg_image_width or height != @bg_image_height

        # Previously calculated @bg_image is OK, return that
        return @bg_image
      end

      @bg_image_width=width
      @bg_image_height=height

      @bg_image_schedule_boundaries = schedule_boundaries

      if output_immediate
        cairo = @cairo
      else
        # @bg_image itself will be generated from surf
        surf = Cairo::ImageSurface.new(Cairo::FORMAT_ARGB32, width,height)
        cairo = Cairo::Context.new(surf)
      end

      cairo.set_line_join(Cairo::LINE_JOIN_ROUND)
      cairo.set_line_cap(Cairo::LINE_CAP_ROUND)
      cairo.set_line_width(LINE_WIDTH)

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
      unless @logo.nil? or output_immediate
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
        cairo.move_to step_width*(i-1)+3, (0.2 * step_height).to_i - LINE_WIDTH
        font = "#{DAY_NAME_FONT} #{font_size}"
        cairo.pango_render_text((step_width)-6, font,
                      "#{DAY_NAMES[(days-i-1+@start_day)%7]}")
      end

      # Draw hour labels and appropriate lines
      hour_segments.downto 1 do |i|
        # compute if this is a major line, and if so make it full width
        # otherwise make it narrow
        if (i % MAJOR_HOUR == MAJOR_MOD)
          cairo.set_line_width(LINE_WIDTH)
        else
          cairo.set_line_width(LINE_WIDTH * 0.20)
        end

        cairo.move_to 0, (step_height*i).to_i
        cairo.rel_line_to width, 0
        cairo.stroke
        cairo.move_to width-step_width+3, (step_height*(i+0.1)).to_i - LINE_WIDTH

        font = "#{HOUR_FONT} #{font_size}"
        hour_text = to_time(@start_hour+(JUMP_HOUR*(i-1)))
        cairo.pango_render_text((step_width)-6, font, hour_text)
      end

      unless output_immediate
        @bg_image = Cairo::SurfacePattern.new(surf)
        return @bg_image
      end
    end

    def draw_sched(pdf_filename = nil)
      @use_pdf_measurements = !pdf_filename.nil?

      start_hours = @events.collect { |ev| ev.hour }
      start_hours << MAX_START_HOUR
      end_hours = @events.collect { |ev| ev.hour + ev.length }
      end_hours << MIN_END_HOUR
      busy_days = @events.collect { |ev| ev.day }
      if TTime::Settings.instance[:show_full_week]
        busy_days << MAX_START_DAY << MIN_END_DAY
      end

      @start_hour = start_hours.min
      @end_hour = end_hours.max
      @start_day = busy_days.min
      @end_day = busy_days.max

      if pdf_filename
        surf = Cairo::PDFSurface.new(pdf_filename, PDF_Width, PDF_Height)
        @cairo = Cairo::Context.new(surf)
        get_bg_image true
      else
        get_cairo
        bg_grid = get_bg_image
        @cairo.set_source bg_grid
        @cairo.rectangle(0,0,width,height)
        @cairo.fill
      end

      compute_layers unless @computed_layers

      @events.each do |i|
        draw_item i
      end

      if pdf_filename
        @cairo.show_page
        surf.finish
      end
    end
  end
end
