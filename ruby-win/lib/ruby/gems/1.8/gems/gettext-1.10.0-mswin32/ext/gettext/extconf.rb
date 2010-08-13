=begin
  extconf.rb

  Copyright (C) 2002-2006 Masao Mutoh <mutoh@highway.ne.jp>
 
  You may redistribute it and/or modify it under the same
  license terms as Ruby.

 $Id: extconf.rb,v 1.1 2006/02/07 00:24:45 mutoh Exp $
=end

require 'mkmf'

if RUBY_PLATFORM =~ /cygwin|mingw|mswin32|bccwin32/
  have_header 'windows.h'
else
  have_func 'setlocale'
  have_func 'nl_langinfo'
end
create_makefile 'locale_system'
