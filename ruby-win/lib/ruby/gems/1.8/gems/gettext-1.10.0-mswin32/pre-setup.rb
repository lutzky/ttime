=begin
  pre-setup.rb

  Copyright(c) 2005,2006 Masao Mutoh
  This program is licenced under the same licence as Ruby.

  $Id: pre-setup.rb,v 1.6 2006/05/13 17:20:52 mutoh Exp $
=end

require 'fileutils'

ruby = config("ruby-path")

gettext = "#{File.join(config("siterubyver"), "gettext.rb")}"
gettext_dir = "#{File.join(config("siterubyver"), "gettext")}"
gettext_dir2 = "#{File.join(config("siterubyverarch"), "gettext")}"
locale = "#{File.join(config("siterubyverarch"), "_locale.so")}"
FileUtils.rm_f gettext if FileTest.exist?(gettext)
FileUtils.rm_rf gettext_dir if FileTest.exist?(gettext_dir)
FileUtils.rm_rf gettext_dir2 if FileTest.exist?(gettext_dir2)
FileUtils.rm_f locale if FileTest.exist?(locale)

