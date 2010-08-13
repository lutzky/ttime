############################################################################
# tc_windows_api.rb
# 
# Test case for the Windows::API class. You should run this as Rake task,
# i.e. 'rake test', instead of running it directly.
############################################################################
require 'windows/api'
require 'test/unit'
include Windows

module Windows
   module Test
      API.auto_namespace = 'Windows::Test'
      API.auto_unicode   = true
      API.auto_method    = true
      API.auto_constant  = true
      $test_method = API.new('GetCurrentDirectory', 'PP', 'L')
   end

   module Foo
      API.auto_namespace = 'Windows::Foo'
      API.auto_unicode  = false
      API.auto_method   = false
      API.auto_constant = false
      $foo_method = API.new('GetSystemDirectory', 'PL', 'L')
   end

   module Bar
      API.auto_namespace = 'Windows::Bar'
      API.auto_constant = true
      API.auto_method   = true
      $bar_method = API.new('GetUserName', 'PP', 'I', 'advapi32')
   end
end

class TC_Windows_API < Test::Unit::TestCase
   include Windows::Test
   include Windows::Foo
   include Windows::Bar

   def setup
      @buf = 0.chr * 256
   end

   def test_version
      assert_equal('0.2.0', API::VERSION)
   end

   def test_auto_unicode
      assert_not_nil(Windows::Bar::GetUserName)
      assert_equal(true, self.respond_to?(:GetUserName))
      assert_equal(false, self.respond_to?(:GetUserNameA))
      assert_equal(false, self.respond_to?(:GetUserNameW))
   end

   def test_auto_constant
      assert_not_nil(Windows::Test::GetCurrentDirectory)
      assert_not_nil(Windows::Bar::GetUserName)

      assert_kind_of(Win32::API, Windows::Test::GetCurrentDirectory)
      assert_respond_to(Windows::Test::GetCurrentDirectory, :call)
   end

   def test_auto_method
      assert_respond_to(self, :GetCurrentDirectory)
      assert_respond_to(self, :GetCurrentDirectoryA)
      assert_respond_to(self, :GetCurrentDirectoryW)

      assert_equal(false, self.respond_to?(:GetSystemDirectory))
      assert_equal(false, self.respond_to?(:GetSystemDirectoryA))
      assert_equal(false, self.respond_to?(:GetSystemDirectoryW))
   end

   def test_call
      assert_respond_to($test_method, :call)
      assert_respond_to($foo_method, :call)
      assert_nothing_raised{ $test_method.call(@buf.length, @buf) }
      assert_nothing_raised{ $foo_method.call(@buf, @buf.length) }
   end
   
   def test_dll_name
      assert_respond_to($test_method, :dll_name)
      assert_equal('kernel32', $test_method.dll_name)
   end
   
   def test_function_name
      assert_respond_to($test_method, :function_name)
      assert_equal('GetCurrentDirectory', $test_method.function_name)
   end
   
   def test_prototype
      assert_respond_to($test_method, :prototype)
      assert_equal('PP', $test_method.prototype)
   end
   
   def test_return_type
      assert_respond_to($test_method, :return_type)
      assert_equal('L', $test_method.return_type)
   end
   
   def teardown
      @buf = nil
   end
end
