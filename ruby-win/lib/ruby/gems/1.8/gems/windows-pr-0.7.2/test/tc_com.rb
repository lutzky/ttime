#####################################################################
# tc_com.rb
#
# Test case for the Windows::COM module.
#####################################################################
require "windows/com"
require "test/unit"

class COMFoo
   include Windows::COM
end

class TC_Windows_COM < Test::Unit::TestCase
   def setup
      @foo = COMFoo.new
   end
   
   def test_method_constants
      assert_not_nil(COMFoo::BindMoniker)
      assert_not_nil(COMFoo::CLSIDFromProgID)
      assert_not_nil(COMFoo::CLSIDFromProgIDEx)
      assert_not_nil(COMFoo::CoAddRefServerProcess)
      assert_not_nil(COMFoo::CoAllowSetForegroundWindow)
      assert_not_nil(COMFoo::CoCancelCall)
      assert_not_nil(COMFoo::CoCopyProxy)
      assert_not_nil(COMFoo::CoCreateFreeThreadedMarshaler)
      assert_not_nil(COMFoo::CoCreateGuid)
      assert_not_nil(COMFoo::CoCreateInstance)
   end

   def teardown
      @foo = nil
   end
end
