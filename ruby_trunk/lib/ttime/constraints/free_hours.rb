require 'ttime/constraints'
require 'ttime/logic/times'

GetText::bindtextdomain("ttime", "locale", nil, "utf-8")

module TTime
  module Constraints
    class FreeHours < AbstractConstraint
      def initialize
        super

      end

      def settings
        Settings.instance[:free_hours] ||= {}
        Settings.instance[:free_hours]
      end

      def chosen_free_hours
        settings[:chosen_free_hours] ||= []
        settings[:chosen_free_hours]
      end

      MinuteIncrement = 15

      menu_item :add_free_hour_dialog, _('Add free hour')

      def gtk_time_setter label_text
        hbox = Gtk::HBox.new
        hbox.pack_start Gtk::Label.new(label_text)

        clock_hbox = Gtk::HBox.new
        clock_hbox.direction = Gtk::Widget::TEXT_DIR_LTR
        hbox.pack_end clock_hbox

        hours = Gtk::SpinButton.new 0, 23, 1
        minutes = Gtk::SpinButton.new 0, 59, MinuteIncrement
        clock_hbox.pack_start hours
        clock_hbox.pack_start Gtk::Label.new(':')
        clock_hbox.pack_start minutes

        return hbox, hours, minutes
      end

      def add_free_hour_dialog params
        dlg = Gtk::Dialog.new _('Add free hour'), nil,
          Gtk::Dialog::MODAL | Gtk::Dialog::DESTROY_WITH_PARENT,
            [ Gtk::Stock::OK, Gtk::Dialog::RESPONSE_ACCEPT ],
            [ Gtk::Stock::CANCEL, Gtk::Dialog::RESPONSE_REJECT ]

        day_combo_box = Gtk::ComboBox.new

        Logic::Day::DAY_NAMES[0..4].each do |s|
          day_combo_box.append_text s
        end

        dlg.vbox.pack_start day_combo_box
        day_combo_box.active = [ 0, params[:day] - 1 ].max

        start_hours = params[:hour] / 100
        start_minutes = params[:hour] % 100

        end_hours = start_hours + 1
        end_minutes = start_minutes

        hbox_start, spin_start_hours, spin_start_minutes = gtk_time_setter _("Start:")
        spin_start_hours.value = start_hours
        spin_start_minutes.value = start_minutes

        hbox_end, spin_end_hours, spin_end_minutes = gtk_time_setter _("End:")
        spin_end_hours.value = end_hours
        spin_end_minutes.value = end_minutes

        dlg.vbox.pack_start hbox_start
        dlg.vbox.pack_start hbox_end

        dlg.signal_connect('response') do |dlg, response|
          if response == Gtk::Dialog::RESPONSE_ACCEPT
            day = day_combo_box.active + 1
            start_at = 100 * spin_start_hours.value + spin_start_minutes.value
            end_at = 100 * spin_end_hours.value + spin_end_minutes.value
            chosen_free_hours << [ day, start_at.to_i , end_at.to_i ]
            update_list
          end
          dlg.destroy
        end

        dlg.show_all
      end

      def evaluate_schedule
        event_list.each do |ev|
          chosen_free_hours.each do |fh|
            fh_day, fh_start, fh_end = fh
            if fh_day == ev.day
              ev_arr = [ev.day, ev.start, ev.end]

              # Make sure 0:30-1:30 and 1:30-2:30 aren't considered
              # overlapping
              ev_range = (ev.start..(ev.end - 1))

              return false if ev_range.include? fh_start
              return false if ev_range.include? fh_end
              return false if fh_start <= ev.start and fh_end >= ev.end
            end
          end
        end

        return true
      end

      def name
        _('Free Hours')
      end

      def preferences_panel
        vbox = Gtk::VBox.new

        @model = Gtk::ListStore.new String, Array
        @treeview = Gtk::TreeView.new(@model)

        @treeview.insert_column(-1, _('Time'),
                                Gtk::CellRendererText.new,
                                'text' => 0)

        update_list

        lbl = Gtk::Label.new
        lbl.markup = "<b>" + _("Free hours") + ":</b>"
        vbox.pack_start lbl, false
        vbox.pack_start @treeview

        hbox = Gtk::HBox.new

        btn_remove = Gtk::Button.new _('Remove')
        btn_remove.signal_connect('clicked') do |_|
          selected_iter = @treeview.selection.selected
          if selected_iter
            chosen_free_hours.delete selected_iter[1]
            update_list
          end
        end

        vbox.pack_end hbox, false
        vbox.pack_end btn_remove, false

        vbox
      end

      def update_list
        @model.clear

        chosen_free_hours.each do |free_hour|
          iter = @model.append
          day, start_time, end_time = free_hour
          iter[0] = "#{Logic::Day.numeric_to_human(day)} " \
                    "#{Logic::Hour.military_to_human(start_time)}-" \
                    "#{Logic::Hour.military_to_human(end_time)} "
          iter[1] = free_hour
        end
      end
    end
  end
end
