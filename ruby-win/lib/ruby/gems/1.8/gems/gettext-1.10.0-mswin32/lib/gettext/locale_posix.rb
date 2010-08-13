=begin
  locale_posix.rb 

  Copyright (C) 2002-2006  Masao Mutoh

  You may redistribute it and/or modify it under the same
  license terms as Ruby.

  $Id: locale_posix.rb,v 1.5 2006/12/14 16:35:57 mutoh Exp $
=end


module Locale 
  # Locale::SystemPosix module for Posix OS (Unix)
  # This is a low-level class. Application shouldn't use this directly.
  module SystemPosix
    extend Locale::System
    module_function
    @@default_locale = Locale::Object.new("C", nil, "UTF-8")

    # Gets the system locale using setlocale and nl_langinfo. 
    # * Returns the system locale (Locale::Object).
    def system
      locale = nil
      [ENV["LC_ALL"], ENV["LC_MESSAGES"], ENV["LANG"], 
	@@default_locale.orig_str].each do |loc|
	if loc != nil and loc.size > 0
	  locale = Locale::Object.new(loc)
	  locale.charset = get_charset(locale)
	  break
	end
      end
      locale
    end

    # Gets the charset of the locale.
    # * locale: Locale::Object
    # * Returns: the charset of the locale
    def get_charset(locale)
      old = set(Locale::System::CTYPE, nil)
      set(Locale::System::CTYPE, locale.orig_str)
      ret = codeset
      set(Locale::System::CTYPE, old)
      ret
    end
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

  if defined? Locale::System::MESSAGES
    CTYPE    = Locale::System::CTYPE    #:nodoc:
    NUMERIC  = Locale::System::NUMERIC  #:nodoc:
    TIME     = Locale::System::TIME     #:nodoc:
    COLLATE  = Locale::System::COLLATE  #:nodoc:
    MONETARY = Locale::System::MONETARY #:nodoc:
    MESSAGES = Locale::System::MESSAGES #:nodoc:
    ALL      = Locale::System::ALL      #:nodoc:
  end
  @@locale_system_module = SystemPosix
end

