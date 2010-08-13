# blog_controller.rb - a sample script for Ruby on Rails
#
# Copyright (C) 2005 Masao Mutoh
#
# This file is distributed under the same license as Ruby-GetText-Package.

class BlogController < ApplicationController
  # If you want to have textdomains each as controllers.
  # You need to bind textdomain here.

  #init_gettext "blog"

  def index
    list
    render :action => 'list'
  end

  def list
    @articles = Article.find(:all, :order => 'lastupdate desc, id desc')
  end

  def show
    @article = Article.find(params[:id])
  end

  def new
    @article = Article.new
  end

  def create
    @article = Article.new(params[:article])
    if @article.save
      flash[:notice] = _('Article was successfully created.')
      redirect_to :action => 'list'
    else
      render :action => 'new'
    end
  end

  def edit
    @article = Article.find(params[:id])
  end

  def update
    @article = Article.find(params[:id])
    if @article.update_attributes(params[:article])
      flash[:notice] = _('Article was successfully updated.')
      redirect_to :action => 'show', :id => @article
    else
      render :action => 'edit'
    end
  end

  def destroy
    Article.find(params[:id]).destroy
    redirect_to :action => 'list'
  end
end
