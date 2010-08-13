=begin
  locale_cgi.rb 

  Copyright (C) 2002-2006  Masao Mutoh

  You may redistribute it and/or modify it under the same
  license terms as Ruby.

  $Id: locale_cgi.rb,v 1.7 2006/12/14 16:35:57 mutoh Exp $
=end


module Locale
  # Locale::System module for CGI.
  # This is a low-level class. Application shouldn't use this directly.
  module SystemCGI
    @@default_locale = Locale::Object.new("en", nil, "UTF-8")
    @@cgi = nil

    module_function
    # Gets the default locale using setlocale and nl_langinfo. 
    # * Returns the system locale (Locale::Object).
    def system
      return @@default_locale unless @@cgi
      cgi_ = cgi
      if ret = cgi_["lang"] and ret.size > 0
      elsif ret = cgi_.cookies["lang"][0]
      elsif lang = cgi_.accept_language and lang.size > 0
	num = lang.index(/;|,/)
	ret = num ? lang[0, num] : lang
      else
	ret = @@default_locale.to_str
      end
 
      codesets = cgi_.accept_charset
      if codesets and codesets.size > 0
	num = codesets.index(',')
	codeset = num ? codesets[0, num] : codesets
	codeset = @@default_locale.charset if codeset == "*"
      else
	codeset = @@default_locale.charset
      end
      Locale::Object.new(ret, nil, codeset)
    end

    # Gets the charset of the locale.
    # * locale: Locale::Object
    # * Returns: the charset of the locale
    def get_charset(locale)
      locale.charset ? locale.charset : @@default_locale.charset
    end

    # Sets a CGI object.
    # * cgi_: CGI object
    # * Returns: self
    def set_cgi(cgi_)
      @@cgi = cgi_
      self
    end
    
    # Sets a CGI object.
    # * cgi_: CGI object
    # * Returns: cgi_ 
    def cgi=(cgi_)
      set_cgi(cgi_)
      cgi_
    end

    # Gets the CGI object. If it is nil, returns new CGI object.
    # * Returns: the CGI object
    def cgi
      @@cgi = CGI.new unless @@cgi
      @@cgi
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
  @@locale_system_module = SystemCGI
end

