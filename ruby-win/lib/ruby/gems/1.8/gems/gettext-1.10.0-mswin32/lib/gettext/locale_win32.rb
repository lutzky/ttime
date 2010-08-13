=begin
  locale_win32.rb

  Copyright (C) 2002-2006  Masao Mutoh <mutoh@highway.ne.jp>

  You may redistribute it and/or modify it under the same
  license terms as Ruby.

  $Id: locale_win32.rb,v 1.16 2007/07/03 16:57:07 mutoh Exp $
=end

require 'gettext/locale_table_win32'

module Locale
  # Locale::SystemWin32 module for win32.
  # This is a low-level class. Application shouldn't use this directly.
  module SystemWin32
    extend Locale::System

    @@default_locale = Locale::Object.new("en", nil, "CP1252")

    module_function
    # Gets the charset of the locale. ENV(LC_ALL > LC_CTYPE > LC_MESSAGES > LANG) > 
    # the default locale from GetUserDefaultLangID.
    # * locale: Locale::Object
    # * Returns the charset of the locale
    def get_charset(locale)
      loc = LocaleTable.find{|v| v[1] == locale.to_win}
      loc = LocaleTable.find{|v| v[1] =~ /^#{locale.language}-/} unless loc
      loc ? loc[2] : "CP1252"
    end

    # Gets the system locale.
    # * Returns the system locale (Locale::Object)
    def system
      lang = nil
      ret = nil
      ["LC_ALL", "LC_CTYPE", "LC_MESSAGES", "LANG"].each do |env|
	lang = ENV[env]
	if lang
	  ret = Locale::Object.new(lang)
	  ret.charset = get_charset(ret)
	  break
	end
      end
      unless lang
	lang = LocaleTable.assoc(locale_id)
	if lang
	  ret = Locale::Object.new(lang[1], nil, lang[2])
	else
	  ret = @@default_locale
	end
      end
      ret
    end

    # Sets a default locale. en.UTF-8 is the default value if not set.
    # * locale: Locale::Object object. You can't set nil.
    # * Returns: self
    def set_default_locale(locale)
      raise "Wrong parameter: #{locale}" if locale.nil?
      @@default_locale = locale
      self
    end
    
    # Sets a default locale. en.UTF-8 is the default value if not set.
    # * locale: Locale::Object
    # * Returns: locale 
    def default_locale=(locale)
      set_default_locale(locale)
      locale
    end

    # Gets the default Locale::Object. 
    # * Returns: the default locale
    def default_locale
      @@default_locale
    end
  end
  @@locale_system_module = SystemWin32
end

