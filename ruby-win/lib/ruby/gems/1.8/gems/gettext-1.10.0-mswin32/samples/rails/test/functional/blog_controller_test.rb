require File.dirname(__FILE__) + '/../test_helper'
require 'blog_controller'

# Re-raise errors caught by the controller.
class BlogController; def rescue_action(e) raise e end; end

class BlogControllerTest < Test::Unit::TestCase
  fixtures :articles

  def setup
    @controller = BlogController.new
    @request    = ActionController::TestRequest.new
    @response   = ActionController::TestResponse.new

    article = Article.find(1)
    article.update_attributes("title"=>"aaaaaaaaaa", 
			      "lastupdate"=> Time.now,
			      "description"=>"aaaaaaaaaa")
    article.save
  end

  def test_index
    get :index
    assert_response :success
    assert_template 'list'
  end

  def test_list
    get :list

    assert_response :success
    assert_template 'list'

    assert_not_nil assigns(:articles)
  end

  def test_show
    get :show, :id => 1

    assert_response :success
    assert_template 'show'

    assert_not_nil assigns(:article)
    assert assigns(:article).valid?
  end

  def test_new
    get :new

    assert_response :success
    assert_template 'new'

    assert_not_nil assigns(:article)
  end

  def test_create
    num_articles = Article.count

    post :create, :article => {
      "title"=>"aaaaaaaaaa", 
      "lastupdate"=> Time.now,
      "description"=>"aaaaaaaaaa"}

    assert_response :redirect
    assert_redirected_to :action => 'list'

    assert_equal num_articles + 1, Article.count
  end

  def test_edit
    get :edit, :id => 1

    assert_response :success
    assert_template 'edit'

    assert_not_nil assigns(:article)
    assert assigns(:article).valid?
  end

  def test_update
    post :update, :id => 1
    assert_response :redirect
    assert_redirected_to :action => 'show', :id => 1
  end

  def test_destroy
    assert_not_nil Article.find(1)

    post :destroy, :id => 1
    assert_response :redirect
    assert_redirected_to :action => 'list'

    assert_raise(ActiveRecord::RecordNotFound) {
      Article.find(1)
    }
  end

end
