begin
  require 'gettext'
  require 'pathname'

  include GetText

  GetText.locale.charset = "UTF-8"

  # Ruby's gettext acts in a sane method - add a path to the set of paths
  # scanned.
  locale_in_data_path = Pathname.new($0).dirname + \
    "../data/locale/%{locale}/LC_MESSAGES/%{name}.mo"
  add_default_locale_path(locale_in_data_path.to_s)
  bound_text_domain = bindtextdomain("ttime")

  # For Glade, however, it only seems to be possible to specify one path at
  # a time. Fortunately, gettext already found it for us.
  my_current_mo = bound_text_domain.entries[0].current_mo
  if my_current_mo
    ENV["GETTEXT_PATH"] = my_current_mo.filename.gsub(
      /locale\/[^\/]+\/LC_MESSAGES.*/,
      "locale/")
  end
rescue LoadError
  def _ s #:nodoc:
    # No gettext? No problem.
    s
  end
end
