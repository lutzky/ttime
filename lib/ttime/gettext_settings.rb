begin
  require 'gettext'
  require 'pathname'

  include GetText

  GetText.locale.charset = "UTF-8"

  # Ruby's gettext acts in a sane method - add a path to the set of paths
  # scanned.
  locale_in_data_path = Pathname.new($0).dirname + \
    "../data/locale/%{lang}/LC_MESSAGES/%{name}.mo"
  if GetText::VERSION >= "2.1.0"
    LocalePath.add_default_rule(locale_in_data_path.to_s)
  else
    add_default_locale_path(locale_in_data_path.to_s)
  end
  bound_text_domain = bindtextdomain("ttime")

  # For Glade, however, it only seems to be possible to specify one path at
  # a time. Fortunately, gettext already found it for us.
  if GetText::VERSION >= "2.1.0"
    _("_File")
    lang = bound_text_domain.mofiles.keys[0]
    my_current_mo = bound_text_domain.mofiles[lang]
  else
    my_current_mo = bound_text_domain.entries[0].current_mo
  end
  if my_current_mo and my_current_mo != :empty
    ENV["GETTEXT_PATH"] = my_current_mo.filename.gsub(
      /locale\/[^\/]+\/LC_MESSAGES.*/,
      "locale/")
  end
rescue Exception => e
  puts "WARNING: Could not activate localized UI: #{e}"
  puts "WARNING: Trace ends at #{e.backtrace[0]}"
  module GetText
    def _ s #:nodoc:
      # No gettext? No problem.
      s
    end
  end
  include GetText
end
