require 'constraints'
require 'set'

GetText::bindtextdomain("ttime", "locale", nil, "utf-8")

module TTime
  module Constraints
    class GroupConstraint < AbstractConstraint
      ( TEXT_COLUMN, SHOW_CHECKBOX_COLUMN, MARKED_COLUMN, \
        COURSE_COLUMN, GROUP_COLUMN ) = (0..4).to_a

      GROUP_TYPE_NAME = {
        :lecture => _('Lecture'),
        :tutorial => _('Tutorial'),
        # TODO More types?
      }

      def initialize
        super

        Settings.instance[:group_constraints] ||= {
          :enabled => false,
          :forbidden_groups => {}
        }
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
          course_iter[TEXT_COLUMN] = course.name

          for group_type in course.groups.collect { |g| g.type }.uniq
            group_type_iter = @model.append(course_iter)
            group_type_iter[TEXT_COLUMN] = GROUP_TYPE_NAME[group_type] or group_type
            for group in course.groups.select { |g| g.type == group_type }
              group_iter = @model.append(group_type_iter)
              group_iter[TEXT_COLUMN] = group.number.to_s
              group_iter[COURSE_COLUMN] = course.number
              group_iter[GROUP_COLUMN] = group.number
              group_iter[SHOW_CHECKBOX_COLUMN] = true

              if group_is_forbidden?(course.number, group.number)
                group_iter[MARKED_COLUMN] = false
              else
                group_iter[MARKED_COLUMN] = true
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

      def tree_setup
        @model = Gtk::TreeStore.new(String, TrueClass, TrueClass, String, \
                                    Fixnum)

        @treeview = Gtk::TreeView.new(@model)
        @treeview.rules_hint = true
        @treeview.selection.mode = Gtk::SELECTION_MULTIPLE

        cellrend = Gtk::CellRendererToggle.new

        cellrend.signal_connect("toggled") do |renderer, path|
          iter = @model.get_iter(path)
          args = iter[COURSE_COLUMN], iter[GROUP_COLUMN]

          if iter[MARKED_COLUMN]
            disallow_group *args
          else
            allow_group *args
          end

          iter[MARKED_COLUMN] ^= true
        end

        @treeview.insert_column(-1, 'Group',
                                Gtk::CellRendererText.new,
                                'text' => TEXT_COLUMN)
        @treeview.insert_column(-1, 'Allowed',
                                cellrend,
                                'visible' => SHOW_CHECKBOX_COLUMN,
                                'active' => MARKED_COLUMN)
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
