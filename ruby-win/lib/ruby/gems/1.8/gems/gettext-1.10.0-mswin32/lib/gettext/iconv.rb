=begin
  iconv.rb - Pseudo Iconv module

  If you don't have iconv but glib2, this library behaves
  as Iconv.iconv.

  Copyright (C) 2004  Masao Mutoh <mutoh@highway.ne.jp>

  You may redistribute it and/or modify it under the same
  license terms as Ruby.

  $Id: iconv.rb,v 1.3 2006/06/11 15:36:20 mutoh Exp $
=end

begin
  require 'iconv.so'
rescue LoadError
  begin
    require 'glib2'
    # Pseudo Iconv module
    # 
    # Provides Iconv.iconv which uses Ruby/GLib(1) functions. This library also required from 'gettext'.
    # If you require 'gettext/iconv', Iconv.iconv try to call Ruby/GLib function 
    # when it doesn't find original Iconv module(iconv.so).
    #
    # (1) Ruby/GLib is a module which is provided from Ruby-GNOME2 Project. 
    # You can get binaries for Win32(One-Click Ruby Installer).
    # <URL: http://ruby-gnome2.sourceforge.jp/>
    module Iconv
      module Failure; end
      class InvalidEncoding < ArgumentError;  include Failure; end
      class IllegalSequence < ArgumentError;  include Failure; end
      class InvalidCharacter < ArgumentError; include Failure; end

      def check_glib_version?(major, minor, micro) # :nodoc:
        (GLib::BINDING_VERSION[0] > major ||
         (GLib::BINDING_VERSION[0] == major && 
          GLib::BINDING_VERSION[1] > minor) ||
         (GLib::BINDING_VERSION[0] == major && 
          GLib::BINDING_VERSION[1] == minor &&
          GLib::BINDING_VERSION[2] >= micro))
      end
      module_function :check_glib_version?

      if check_glib_version?(0, 11, 0)
	# This is a function equivalent of Iconv.iconv.
	# * to: encoding name for destination
	# * from: encoding name for source
	# * str: strings to be converted
	# * Returns: Returns an Array of converted strings.
        def iconv(to, from, str)
          begin
            GLib.convert(str, to, from).split(//)
          rescue GLib::ConvertError => e
            case e.code
            when GLib::ConvertError::NO_CONVERSION
              raise InvalidEncoding.new(str)
            when GLib::ConvertError::ILLEGAL_SEQUENCE
              raise IllegalSequence.new(str)
            else
              raise InvalidCharacter.new(str)
            end
          end
        end
      else
       def iconv(to, from, str) # :nodoc:
         begin
           GLib.convert(str, to, from).split(//)
         rescue
           raise IllegalSequence.new(str)
         end
       end
      end
      module_function :iconv
    end
  rescue LoadError
    module Iconv
      module_function
      def iconv(to, from, str) # :nodoc:
        warn "Iconv was not found." if $DEBUG
        str.split(//)
      end
    end
  end
end

if __FILE__ == $0
  puts Iconv.iconv("EUC-JP", "UTF-8", "ほげ").join
  begin
    puts Iconv.iconv("EUC-JP", "EUC-JP", "ほげ").join
  rescue Iconv::Failure
    puts $!
    puts $!.class
  end
end
