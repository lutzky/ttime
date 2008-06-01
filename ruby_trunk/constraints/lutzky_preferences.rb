require 'constraints'
require 'set'

GetText::bindtextdomain("ttime", "locale", nil, "utf-8")

module TTime
  module Constraints
    class LutzkyPreferences < AbstractConstraint
      ( TEXT_COLUMN, SHOW_CHECKBOX_COLUMN, MARKED_COLUMN, \
        COURSE_COLUMN, TYPE_COLUMN, GROUP_COLUMN ) = (0..6).to_a

      def initialize
        super

        @enabled = false

        @forbidden_groups = {
            "044147" => { :tutorial => Set.new([11,21]) },
            "236360" => { :tutorial => Set.new([12,13]) }
        }

      end

      def evaluate_schedule
        true
      end

      def evaluate_group(grp)
        return true unless @enabled

        flag = true if grp.number == 10

        if @forbidden_groups.include? grp.course.number \
          and @forbidden_groups[grp.course.number].include? grp.type \
          and @forbidden_groups[grp.course.number][grp.type].include? grp.number
              return false
        end

        return true
      end

      def name
        _('Lutzky Preferences')
      end

      def allow_group(course, type, group)
        @forbidden_groups[course][type].delete group
      end

      def disallow_group(course, type, group)
        @forbidden_groups[course] = {} unless @forbidden_groups.include? course

        unless @forbidden_groups[course].include? type
          @forbidden_groups[course][type] = Set.new
        end

        @forbidden_groups[course][type].add group
      end

      def tree_setup
        @model = Gtk::TreeStore.new(String, TrueClass, TrueClass, String, \
                                    Symbol, Fixnum)

        @treeview = Gtk::TreeView.new(@model)
        @treeview.rules_hint = true
        @treeview.selection.mode = Gtk::SELECTION_MULTIPLE

        cellrend = Gtk::CellRendererToggle.new

        cellrend.signal_connect("toggled") do |renderer, path|
          iter = @model.get_iter(path)
          args = iter[COURSE_COLUMN], iter[TYPE_COLUMN].to_sym, iter[GROUP_COLUMN]

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

        selections = \
          [
            [ 'מעגלי מיתוג אלקטרוניים',
              "044147",
              [
                [ "Lectures", :lecture,
                  [
                    [ 10, true ],
                    [ 20, true ]
                  ]
                ],
                [ "Tutorials", :tutorial,
                  [
                    [ 11, false ],
                    [ 12, true ],
                    [ 13, true ],
                    [ 21, false ],
                    [ 22, true ],
                  ]
                ]
              ]
            ],
            [ 'תורת הקומפילציה',
              "236360",
              [
                [ "Lectures", :lecture,
                  [
                    [ 10, true ],
                  ]
                ],
                [ "Tutorials", :tutorial,
                  [
                    [ 11, true ],
                    [ 12, false ],
                    [ 13, false ],
                  ]
                ]
              ]
            ]
          ]

        selections.each do |course|
          course_iter = @model.append(nil)
          course_iter[TEXT_COLUMN] = course[0]
          course_iter[SHOW_CHECKBOX_COLUMN] = false

          course[2].each do |group_type|
            group_type_iter = @model.append(course_iter)
            group_type_iter[TEXT_COLUMN] = group_type[0]
            group_type_iter[SHOW_CHECKBOX_COLUMN] = false

            group_type[2].each do |group|
              group_iter = @model.append(group_type_iter)
              group_iter[TEXT_COLUMN] = group[0].to_s
              group_iter[SHOW_CHECKBOX_COLUMN] = true
              group_iter[MARKED_COLUMN] = group[1]
              group_iter[COURSE_COLUMN] = course[1]
              group_iter[TYPE_COLUMN] = group_type[1]
              group_iter[GROUP_COLUMN] = group[0]
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
