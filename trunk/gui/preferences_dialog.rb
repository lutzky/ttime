require 'gtk2'

module TTime
  module GUI
    class PreferencesDialog < Gtk::Dialog
      def initialize(constraints)
        super("Preferences", nil, nil, [ Gtk::Stock::CLOSE, Gtk::Dialog::RESPONSE_NONE ])

        self.set_default_size 300, 300
        self.modal = true

        @constraints = constraints

        @notebook = Gtk::Notebook.new
        @preferences_notebook = Gtk::Notebook.new
        @constraints_notebook = Gtk::Notebook.new

        @preferences_notebook.tab_pos = 0

        @constraints.each do |c|
          @constraints_notebook.append_page c.preferences_panel,
            Gtk::Label.new(c.name)
        end

        @preferences_notebook.append_page @constraints_notebook,
          Gtk::Label.new('Constraints')

        vbox.pack_start @preferences_notebook

        signal_connect('response') do
          self.destroy
        end

        show_all
      end
    end
  end
end
