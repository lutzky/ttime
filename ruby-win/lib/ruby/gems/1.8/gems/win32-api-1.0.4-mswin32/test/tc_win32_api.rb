############################################################################
# tc_win32_api.rb
# 
# Test case for the Win32::API class. You should run this as Rake task,
# i.e. 'rake test', instead of running it directly.
############################################################################
require 'win32/api'
require 'test/unit'
include Win32

class TC_Win32_API < Test::Unit::TestCase
   def setup
      @buf = 0.chr * 260
      @api = API.new('GetCurrentDirectory', 'LP')
      @gle = API.new('GetLastError', 'V', 'L')
   end

   def test_version
      assert_equal('1.0.4', API::VERSION)
   end
 
   def test_call
      assert_respond_to(@api, :call)
      assert_nothing_raised{ @api.call(@buf.length, @buf) }
      assert_equal(Dir.pwd.tr('/', "\\"), @buf.strip)
   end
   
   def test_call_with_void
      assert_nothing_raised{ @gle.call }
      assert_nothing_raised{ @gle.call(nil) }
   end
   
   def test_dll_name
      assert_respond_to(@api, :dll_name)
      assert_equal('kernel32', @api.dll_name)
   end
   
   def test_function_name
      assert_respond_to(@api, :function_name)
      assert_equal('GetCurrentDirectory', @api.function_name)
   end
   
   def test_prototype
      assert_respond_to(@api, :prototype)
      assert_equal(['L', 'P'], @api.prototype)
   end
   
   def test_return_type
      assert_respond_to(@api, :return_type)
      assert_equal('L', @api.return_type)
   end
   
   def test_constructor_high_iteration
      1000.times{
         assert_nothing_raised{ API.new('GetUserName', 'P', 'P', 'advapi32') }
      }
   end
   
   def test_constructor_expected_failures
      assert_raise(API::Error){ API.new('GetUserName', 'PL', 'I', 'foo') }
      assert_raise(API::Error){ API.new('GetUserName', 'X', 'I', 'foo') }
      assert_raise(API::Error){ API.new('GetUserName', 'PL', 'X', 'foo') }
   end
   
   def test_call_expected_failures
      assert_raise(TypeError){ @api.call('test', @buf) }
   end
   
   def teardown
      @buf = nil
      @api = nil
      @gle = nil
   end
end
