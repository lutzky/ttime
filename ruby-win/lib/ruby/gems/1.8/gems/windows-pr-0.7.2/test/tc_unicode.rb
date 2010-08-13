#####################################################################
# tc_unicode.rb
#
# Test case for the Windows::Unicode module.
#####################################################################
require "windows/unicode"
require "test/unit"

class UnicodeFoo
   include Windows::Unicode
end

class TC_Windows_Unicode < Test::Unit::TestCase
   def setup
      @foo = UnicodeFoo.new
   end

   def test_numeric_constants
      assert_equal(0, UnicodeFoo::CP_ACP)
      assert_equal(1, UnicodeFoo::CP_OEMCP)
      assert_equal(2, UnicodeFoo::CP_MACCP)
      assert_equal(3, UnicodeFoo::CP_THREAD_ACP)
      assert_equal(42, UnicodeFoo::CP_SYMBOL)
      assert_equal(65000, UnicodeFoo::CP_UTF7)
      assert_equal(65001, UnicodeFoo::CP_UTF8)

      assert_equal(0x00000001, UnicodeFoo::MB_PRECOMPOSED)
      assert_equal(0x00000002, UnicodeFoo::MB_COMPOSITE)
      assert_equal(0x00000004, UnicodeFoo::MB_USEGLYPHCHARS)
      assert_equal(0x00000008, UnicodeFoo::MB_ERR_INVALID_CHARS)

      assert_equal(0x00000200, UnicodeFoo::WC_COMPOSITECHECK)
      assert_equal(0x00000010, UnicodeFoo::WC_DISCARDNS)
      assert_equal(0x00000020, UnicodeFoo::WC_SEPCHARS)
      assert_equal(0x00000040, UnicodeFoo::WC_DEFAULTCHAR)
      assert_equal(0x00000400, UnicodeFoo::WC_NO_BEST_FIT_CHARS)
   end

   def test_method_constants
      assert_not_nil(UnicodeFoo::GetTextCharset)
      assert_not_nil(UnicodeFoo::GetTextCharsetInfo)
      assert_not_nil(UnicodeFoo::IsDBCSLeadByte)
      assert_not_nil(UnicodeFoo::IsDBCSLeadByteEx)
      assert_not_nil(UnicodeFoo::IsTextUnicode)
      assert_not_nil(UnicodeFoo::MultiByteToWideChar)
      assert_not_nil(UnicodeFoo::TranslateCharsetInfo)
      assert_not_nil(UnicodeFoo::WideCharToMultiByte)
   end

   def teardown
      @foo  = nil
      @unicode = nil
   end
end
