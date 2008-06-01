require 'constraints'

GetText::bindtextdomain("ttime", "locale", nil, "utf-8")

module TTime
  module Constraints
    class LutzkyPreferences < AbstractConstraint
      ( TEXT_COLUMN, SHOW_CHECKBOX_COLUMN, MARKED_COLUMN ) = (0..2).to_a

      def initialize
        super

        @enabled = false

        @allowed_groups = {
            "044147" => { :lecture => [10,20], :tutorial => [12,13,22] },
            "236360" => { :tutorial => [11] }
        }
      end

      def evaluate_schedule
        true
      end

      def evaluate_group(grp)
        return true unless @enabled

        @allowed_groups.each do |course, selections|
          if grp.course.number == course
            if selections.include?(grp.type)
              return false unless selections[grp.type].include? grp.number
            end
          end
        end

        true
      end

      def name
        _('Lutzky Preferences')
      end

      def tree_setup
        @model = Gtk::TreeStore.new(String, TrueClass, TrueClass)

        @treeview = Gtk::TreeView.new(@model)
        @treeview.rules_hint = true
        @treeview.selection.mode = Gtk::SELECTION_MULTIPLE

        @treeview.insert_column(-1, 'Group',
                                Gtk::CellRendererText.new,
                                'text' => TEXT_COLUMN)
        @treeview.insert_column(-1, 'Allowed',
                                Gtk::CellRendererToggle.new,
                                'visible' => SHOW_CHECKBOX_COLUMN,
                                'active' => MARKED_COLUMN)

        selections = \
          [
            [ 'Compilation',
              [
                [ 'Tutorials',
                  [
                    [ 11, true ],
                    [ 12, false ]
                  ]
                ]
              ]
            ]
          ]

        selections.each do |course|
          course_iter = @model.append(nil)
          course_iter[TEXT_COLUMN] = course[0]
          course_iter[SHOW_CHECKBOX_COLUMN] = false

          course[1].each do |group_type|
            group_type_iter = @model.append(course_iter)
            group_type_iter[TEXT_COLUMN] = group_type[0]
            group_type_iter[SHOW_CHECKBOX_COLUMN] = false

            group_type[1].each do |group|
              group_iter = @model.append(group_type_iter)
              group_iter[TEXT_COLUMN] = group[0].to_s
              group_iter[SHOW_CHECKBOX_COLUMN] = true
              group_iter[MARKED_COLUMN] = group[1]
            end
          end
        end
      end

      def preferences_panel
        vbox = Gtk::VBox.new

        sw = Gtk::ScrolledWindow.new(nil, nil)
        sw.shadow_type = Gtk::SHADOW_ETCHED_IN
        sw.set_policy(Gtk::POLICY_AUTOMATIC, Gtk::POLICY_AUTOMATIC)

        tree_setup

        sw.add(@treeview)

        btn_enabled = Gtk::CheckButton.new(_('I am Lutzky'))
        btn_enabled.active = @enabled

        btn_enabled.signal_connect('toggled') do
          @enabled = btn_enabled.active?
        end
        vbox.pack_start btn_enabled, false, false

        vbox.pack_end sw, true, true, 0

        vbox
      end
    end
  end
end
