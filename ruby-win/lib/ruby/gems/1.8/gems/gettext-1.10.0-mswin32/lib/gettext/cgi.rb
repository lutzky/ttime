=begin
  gettext/cgi.rb - GetText for CGI

  Copyright (C) 2005  Masao Mutoh

  You may redistribute it and/or modify it under the same
  license terms as Ruby.

  $Id: cgi.rb,v 1.6 2006/02/20 12:35:06 mutoh Exp $
=end

require 'cgi'
require 'gettext'
require 'gettext/locale_cgi'

module Locale
  module_function
  # Sets a CGI object.
  # * cgi_: CGI object
  # * Returns: self
  def set_cgi(cgi_)
    @@locale_system_module.set_cgi(cgi_)
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
    @@locale_system_module.cgi
  end
end

module GetText
  module_function

  # Sets a CGI object.
  # * cgi_: CGI object
  # * Returns: self
  def set_cgi(cgi_)
    Locale.set_cgi(cgi_)
  end

  # Same as GetText.set_cgi.
  # * cgi_: CGI object
  # * Returns: cgi_
  def cgi=(cgi_)
    set_cgi(cgi_)
    cgi_
  end

  # Gets the CGI object. If it is nil, returns new CGI object.
  # * Returns: the CGI object
  def cgi
    Locale.cgi
  end
end
