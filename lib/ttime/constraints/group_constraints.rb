require 'ttime/constraints'
require 'ttime/gettext_settings'
require 'set'

module TTime
  module Constraints
    class GroupConstraint < AbstractConstraint
      RIGHT_BUTTON = 3

      COLUMNS = [
        [ :text, String ],
        [ :show_checkbox, TrueClass ],
        [ :marked, TrueClass ],
        [ :course, String ],
        [ :group, Fixnum ],
        [ :time, String ],
        [ :lecturer, String ],
      ]

      def col_index(column_name)
        COLUMNS.each_index do |i|
          return i if COLUMNS[i][0] == column_name
        end
        raise Exception("Column not found")
      end

      def column_classes
        COLUMNS.collect { |col| col[1] }
      end

      menu_item :mark_this_group, _("Mark this group"), true

      def mark_this_group params
        group = params[:data][:event].group
        course = group.course
        course.groups.each do |grp|
          if grp.type == group.type
            disallow_group course.number, grp.number
          end
        end
        allow_group course.number, group.number

        TTime::GUI::MainWindow.instance.reject_events_from_calendar! do |data|
          ev = data[:event]
          ev.course.number == course.number and \
            ev.group.type == group.type and \
            ev.group.number != group.number
        end

        update_forbidden_marks
      end

      def initialize
        super

        Settings.instance[:group_constraints] ||= {
          :enabled => true,
          :forbidden_groups => {}
        }
      end

      def each_selection_iter(start_at = nil, &block)
        block.call start_at if start_at
        if start_at
          iter = start_at.first_child
          if iter
            each_selection_iter(iter, &block)
            while iter.next!
              each_selection_iter iter, &block
            end
          end
        else
          iter = @model.iter_first
          if iter
            each_selection_iter(iter, &block)
            while iter.next!
              each_selection_iter iter, &block
            end
          end
        end
      end

      def update_forbidden_marks
        each_selection_iter do |iter|
          course_number = iter[col_index(:course)]
          group_number = iter[col_index(:group)]
          iter[col_index(:marked)] = \
            !group_is_forbidden?(course_number, group_number)
        end
      end

      def settings
        Settings.instance[:group_constraints]
      end

      def forbidden_groups
        return settings[:forbidden_groups]
      end

      def evaluate_schedule
        true
      end

      def evaluate_group(grp)
        return true unless settings[:enabled]
        return !(group_is_forbidden?(grp.course.number, grp.number))
      end

      def name
        _('Group Constraints')
      end

      def allow_group(course_number, group_number)
        forbidden_groups[course_number].delete group_number
      end

      def update_courses(course_list)
        @model.clear

        for course in course_list
          course_iter = @model.append(nil)
          course_iter[col_index(:text)] = course.name

          for group_type in course.groups.collect { |g| g.type }.uniq
            group_type_iter = @model.append(course_iter)
            group_type_iter[col_index(:text)] = TTime::Data::GROUP_TYPES[group_type] or group_type
            for group in course.groups.select { |g| g.type == group_type }
              group_iter = @model.append(group_type_iter)
              group_iter[col_index(:text)] = group.number.to_s
              group_iter[col_index(:course)] = course.number
              group_iter[col_index(:group)] = group.number
              group_iter[col_index(:show_checkbox)] = true
              group_iter[col_index(:time)] = group.time_as_text
              if group.description
                group_iter[col_index(:lecturer)] = group.description
              else
                group_iter[col_index(:lecturer)] = group.lecturer
              end

              if group_is_forbidden?(course.number, group.number)
                group_iter[col_index(:marked)] = false
              else
                group_iter[col_index(:marked)] = true
              end
            end
          end
        end
      end

      def group_is_forbidden?(course_number, group_number)
        return false unless forbidden_groups.include?(course_number)
        return forbidden_groups[course_number].include?(group_number)
      end

      def disallow_group(course_number, group_number)
        forbidden_groups[course_number] ||= Set.new
        forbidden_groups[course_number].add group_number
      end

      def recursively_mark iter, marked
        iter[col_index(:marked)] = marked
        if iter.has_child?
          sub_iter = iter.first_child
          recursively_mark sub_iter, marked
          while sub_iter.next!
            recursively_mark sub_iter, marked
          end
        end
      end

      def select_all mark_with = true
        selection = @treeview.selection
        return if selection.nil?
        if selection.selected.has_child?
          iter = selection.selected
        else
          iter = selection.selected.parent
        end

        recursively_mark iter, mark_with
      end

      def select_none
        select_all false
      end

      def tree_setup
        @model = Gtk::TreeStore.new(*column_classes)
        @treeview = Gtk::TreeView.new(@model)
        @treeview.rules_hint = true
        @treeview.selection.mode = Gtk::SELECTION_SINGLE

        @treeview.signal_connect("button-press-event") do |treeview, e|
          if e.button == RIGHT_BUTTON
            menu = Gtk::Menu.new
            [
              [ _("Select all"),  lambda { select_all } ],
              [ _("Select none"), lambda { select_none } ],
            ].each do |label, block|
              mi = Gtk::MenuItem.new label
              mi.signal_connect("activate", &block)
              menu.append mi
            end
            menu.show_all
            menu.popup nil, nil, 3, e.time
            false
          end
        end

        cellrend = Gtk::CellRendererToggle.new

        cellrend.signal_connect("toggled") do |renderer, path|
          iter = @model.get_iter(path)
          args = iter[col_index(:course)], iter[col_index(:group)]

          if iter[col_index(:marked)]
            disallow_group(*args)
          else
            allow_group(*args)
          end

          iter[col_index(:marked)] ^= true
        end

        @treeview.insert_column(-1, _('Allowed'),
                                cellrend,
                                'visible' => col_index(:show_checkbox),
                                'active' => col_index(:marked))
        @treeview.insert_column(-1, _('Group'),
                                Gtk::CellRendererText.new,
                                'text' => col_index(:text))
        @treeview.insert_column(-1, _('Time'),
                                Gtk::CellRendererText.new,
                                'text' => col_index(:time))
        @treeview.insert_column(-1, _('Lecturer'),
                                Gtk::CellRendererText.new,
                                'text' => col_index(:lecturer))
      end

      def enabled?
        settings[:enabled]
      end

      def preferences_panel
        vbox = Gtk::VBox.new

        sw = Gtk::ScrolledWindow.new(nil, nil)
        sw.shadow_type = Gtk::SHADOW_ETCHED_IN
        sw.set_policy(Gtk::POLICY_AUTOMATIC, Gtk::POLICY_AUTOMATIC)

        tree_setup

        @treeview.sensitive = settings[:enabled]

        sw.add(@treeview)

        btn_enabled = Gtk::CheckButton.new(_('Use group constraints'))
        btn_enabled.active = settings[:enabled]

        btn_enabled.signal_connect('toggled') do
          settings[:enabled] = btn_enabled.active?
          @treeview.sensitive = settings[:enabled]
        end

        vbox.pack_start btn_enabled, false, false

        vbox.pack_end sw, true, true, 0

        vbox
      end
    end
  end
end
