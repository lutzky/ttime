# application.rb - a sample script for Ruby on Rails
#
# Copyright (C) 2005,2006 Masao Mutoh
#
# This file is distributed under the same license as Ruby-GetText-Package.

class ApplicationController < ActionController::Base
  # Initialize GetText and Content-Type.
  # You need to call this once a request from WWW browser.
  # You can select the scope of the textdomain.
  # 1. If you call init_gettext in ApplicationControler,
  #    The textdomain apply whole your application.
  # 2. If you call init_gettext in each controllers
  #    (In this sample, blog_controller.rb is applicable)
  #    The textdomains are applied to each controllers/views.
  init_gettext "blog"  # textdomain, charset = "UTF-8", content_type = "text/html" 

=begin
  # You can set callback methods. These methods are called on the each WWW request.
  def before_init_gettext(cgi)
    p "before_init_gettext"
  end
  def after_init_gettext(cgi)
    p "after_init_gettext"
  end
=end

=begin
# you can redefined the title/explanation of the top of the error message.
  ActionView::Helpers::ActiveRecordHelper::L10n.set_error_message_title(N_("An error is occured on %{record}"), N_("%{num} errors are occured on %{record}"))
  ActionView::Helpers::ActiveRecordHelper::L10n.set_error_message_explanation(N_("The error is:"), N_("The errors are:"))
=end
end
