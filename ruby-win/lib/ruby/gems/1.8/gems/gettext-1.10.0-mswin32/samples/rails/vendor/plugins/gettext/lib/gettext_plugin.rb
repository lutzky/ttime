# gettext_plugin.rb - a sample script for Ruby on Rails
#
# Copyright (C) 2005,2006 Masao Mutoh
#
# This file is distributed under the same license as Ruby-GetText-Package.

require 'gettext/rails'

module LangHelper
  include GetText::Rails

  bindtextdomain("gettext_plugin", :path => File.join(RAILS_ROOT, "vendor/plugins/gettext/locale"))
  
  def show_language
    langs = ["en"] + Dir.glob(File.join(RAILS_ROOT,"locale/*")).collect{|item| File.basename(item)}
    langs.delete("CVS")
    langs.uniq!
    ret = "<h4>" + _("Select locale") + "</h4>"
    langs.sort.each_with_index do |lang, i|
      ret << link_to("[#{lang}]", :action => "cookie_locale", :lang => lang)
      if ((i + 1) % 6 == 0)
	ret << "<br/>"
      end
    end
    ret
  end

  def cookie_locale
    cookies["lang"] = params["lang"]
    flash[:notice] = _('Cookie &quot;lang&quot; is set: %s') % params["lang"]
    redirect_to :action => 'list'
  end
end
 
