$GETTEXT_DOMAIN = nil
begin
  require 'gettext'

  unless Dir["locale"].empty?
    # We have a locale directory here. Set domain to "locale" to use data
    # from there.
    $GETTEXT_DOMAIN = "locale"
  end

  GetText::bindtextdomain("ttime", $GETTEXT_DOMAIN)
rescue LoadError
  module GetText; def _ s; s; end; end
end
