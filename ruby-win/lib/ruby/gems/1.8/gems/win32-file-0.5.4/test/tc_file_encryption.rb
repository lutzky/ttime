#############################################################################
# tc_file_encryption.rb
#
# Test case for the encryption related methods of win32-file. You should
# run this test via the 'rake test' or 'rake test_encryption' task.
# 
# Note: These tests may fail based on the security setup of your system.
#############################################################################
require 'test/unit'
require 'win32/file'

class TC_Win32_File_Encryption < Test::Unit::TestCase
   def setup
      @file  = 'sometestfile.txt'
      @msg   = 'IGNORE - may not work due to security setup of your system'
   end
   
   def test_encrypt
      assert_respond_to(File, :encrypt)
      assert_nothing_raised(@msg){ File.encrypt(@file) }
   end
   
   def test_decrypt
      assert_respond_to(File, :decrypt)
      assert_nothing_raised(@msg){ File.decrypt(@file) }
   end
   
   def teardown
      @file = nil
   end
end