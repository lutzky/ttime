require 'test/unit'

require 'gettext.rb'

require 'testlib1.rb'
require 'testlib2.rb'
require 'testlib3.rb'
require 'testlib4.rb'
require 'testlib6.rb'
require 'test_rubyparser.rb'
require 'test_sgettext.rb'
require 'test_nsgettext.rb'

class TestGetText < Test::Unit::TestCase
  include GetText

  def test_bindtextdomain
    GetText.locale = nil
    return if /linux/ !~ RUBY_PLATFORM

    GetText.bindtextdomain("libc")
    assert_equal("終了しました", GetText._("Terminated"))

    GetText.locale = nil
    GetText.bindtextdomain("test1", "locale")

    assert_equal("japanese", GetText._("language"))
    GetText.bindtextdomain("libc", "/usr/share/locale")
    assert_equal("終了しました", GetText._("Terminated"))

    GetText.bindtextdomain("test2", "locale")
    assert_equal("JAPANESE", GetText._("LANGUAGE"))

    GetText.bindtextdomain("libc", "/usr/share/locale/")
    assert_equal("終了しました", GetText._("Terminated"))

    GetText.locale = nil
    GetText.bindtextdomain("libc", "/usr/share/locale", "de")
    assert_equal("Beendet", GetText._("Terminated"))

    GetText.bindtextdomain("test1", "locale")

    GetText.locale = "fr"
    assert_equal("french", GetText._("language"))

    GetText.locale = nil
    GetText.bindtextdomain("test1", "locale", "ja")
    assert_equal("japanese", GetText._("language"))

  end

  def test_bindtextdomain_include_module
    GetText.locale = nil
    bindtextdomain("test1", "locale")
    assert_equal("japanese", _("language"))
  end

  def test_empty
    set_locale(nil)
    bindtextdomain("test1", "locale")
    assert_equal("japanese", gettext("language"))
    assert_equal("", gettext(""))
    assert_equal("", gettext(nil))
  end

  def test_class
    GetText.locale = "ja_JP.eucJP"
    bindtextdomain("test2", "locale")
    testlib6 = TestLib6.new
    assert_equal("japanese", testlib6.test)
    set_locale("fr")
    assert_equal("japanese", testlib6.test) #no influence of previous line
    assert_equal("ONE IS 1.", testlib6.test_formatted_string)
    testlib6.setlocale("ja")
    assert_equal("FRENCH", _("LANGUAGE")) #no influence of previous line
    assert_equal("japanese", testlib6.test)
  end

  def test_subclass
    GetText.locale = nil
    testlib2 = TestLib2.new
    assert_equal("JAPANESE", testlib2.test2)
    assert_equal("japanese", testlib2.test)
  end

  def test_nested_module
    GetText.locale = nil
    testlib3 = TestLib3::Test3.new
    assert_equal("JAPANESE", testlib3.test3)
  end

  def test_no_msgstr
    GetText.locale = nil
    bindtextdomain("test1", "locale", "ja")
    assert_equal("nomsgstr", _("nomsgstr"))
  end

  def test_complex
    GetText.locale = nil
    testlib4 = TestRubyParser.new
    assert_equal("AAA", testlib4.test_1)
    assert_equal("AAA\n", testlib4.test_2)
    assert_equal("BBB\nCCC", testlib4.test_3)
    assert_equal("BBB
CCC
DDD
", testlib4.test_4)
    assert_equal("EEE", testlib4.test_5)
    assert_equal("EEEfooFFF", testlib4.test_6)
    assert_equal("GGGHHHIII", testlib4.test_7)
  end

  def test_noop
    GetText.locale = nil
    assert_equal("test", N_("test"))
  end

  def test_sgettext
    GetText.locale = nil
    testlib5 = TestSGetText.new

    assert_equal("MATCHED", testlib5.test_1)
    assert_equal("MATCHED", testlib5.test_2)
    assert_equal("AAA", testlib5.test_3)
    assert_equal("CCC", testlib5.test_4)
    assert_equal("CCC", testlib5.test_5)
    assert_equal("BBB", testlib5.test_6)
    assert_equal("B|BB", testlib5.test_7)
    assert_equal("MATCHED", testlib5.test_8)
    assert_equal("BBB", testlib5.test_9)
  end

  def test_nsgettext
    GetText.locale = nil
    testlib5 = TestNSGetText.new
    assert_equal(["single", "plural"], testlib5.test_1)
    assert_equal(["single", "plural"], testlib5.test_2)
    assert_equal(["AAA", "BBB"], testlib5.test_3)
    assert_equal(["CCC", "DDD"], testlib5.test_4)
    assert_equal(["CCC", "DDD"], testlib5.test_5)
    assert_equal(["BBB", "CCC"], testlib5.test_6)
    assert_equal(["B|BB", "CCC"], testlib5.test_7)
    assert_equal(["single", "plural"], testlib5.test_8)
    assert_equal(["BBB", "DDD"], testlib5.test_9)
  end

  def test_plural
    GetText.locale = nil
    bindtextdomain("plural", "locale", "ja")
    assert_equal("all", n_("one", "two", 0))
    assert_equal("all", n_("one", "two", 1))
    assert_equal("all", n_("one", "two", 2))

    setlocale("da")
    assert_equal("da_plural", n_("one", "two", 0))
    assert_equal("da_one", n_("one", "two", 1))
    assert_equal("da_plural", n_("one", "two", 2))

    setlocale("fr")
    assert_equal("fr_one", ngettext("one", "two", 0))
    assert_equal("fr_one", ngettext("one", "two", 1))
    assert_equal("fr_plural", ngettext("one", "two", 2))

    setlocale("la")
    assert_equal("la_one", ngettext("one", "two", 21))
    assert_equal("la_one", ngettext("one", "two", 1))
    assert_equal("la_plural", ngettext("one", "two", 2))
    assert_equal("la_zero", ngettext("one", "two", 0))

    setlocale("ir")
    assert_equal("ir_one", ngettext("one", "two", 1))
    assert_equal("ir_two", ngettext("one", "two", 2))
    assert_equal("ir_plural", ngettext("one", "two", 3))
    assert_equal("ir_plural", ngettext("one", "two", 0))

    setlocale("li")
    assert_equal("li_one", ngettext("one", "two", 1))
    assert_equal("li_two", ngettext("one", "two", 22))
    assert_equal("li_three", ngettext("one", "two", 11))

    setlocale("cr")
    assert_equal("cr_one", ngettext("one", "two", 1))
    assert_equal("cr_two", ngettext("one", "two", 2))
    assert_equal("cr_three", ngettext("one", "two", 5))

    setlocale("po")
    assert_equal("po_one", ngettext("one", "two", 1))
    assert_equal("po_two", ngettext("one", "two", 2))
    assert_equal("po_three", ngettext("one", "two", 5))

    setlocale("sl")
    assert_equal("sl_one", ngettext("one", "two", 1))
    assert_equal("sl_two", ngettext("one", "two", 2))
    assert_equal("sl_three", ngettext("one", "two", 3))
    assert_equal("sl_three", ngettext("one", "two", 4))
    assert_equal("sl_four", ngettext("one", "two", 5))
  end

  def test_plural_format_invalid
    setlocale(nil)
    bindtextdomain("plural_error", "locale", "ja")
    #If it defines msgstr[0] only, msgstr[0] is used everytime.
    assert_equal("a", n_("first", "second", 0)) 
    assert_equal("a", n_("first", "second", 1)) 
    assert_equal("a", n_("first", "second", 2)) 
    # Use default(plural = 0)
    setlocale("fr")
    assert_equal("fr_first", n_("first", "second", 0))   
    assert_equal("fr_first", n_("first", "second", 1))
    assert_equal("fr_first", n_("first", "second", 2))
    setlocale("da")
    assert_equal("da_first", n_("first", "second", 0))   
    assert_equal("da_first", n_("first", "second", 1))
    assert_equal("da_first", n_("first", "second", 2))
    setlocale("la")
    assert_equal("la_first", n_("first", "second", 0))   
    assert_equal("la_first", n_("first", "second", 1))
    assert_equal("la_first", n_("first", "second", 2))
  end

  def test_plural_array
    GetText.locale = nil
    bindtextdomain("plural", "locale", "da")
    assert_equal("da_plural", n_(["one", "two"], 0))
    assert_equal("da_one", n_(["one", "two"], 1))
    assert_equal("da_plural", n_(["one", "two"], 2))
  end

  def test_plural_with_single
    GetText.locale = nil
    bindtextdomain("plural", "locale", "ja")
    assert_equal("hitotsu", _("single"))
    assert_equal("hitotsu", n_("single", "plural", 1))
    assert_equal("hitotsu", n_("single", "plural", 2))
    assert_equal("all", n_("one", "two", 1))
    assert_equal("all", n_("one", "two", 2))
    assert_equal("all", _("one"))

    bindtextdomain("plural", "locale", "fr")
    assert_equal("fr_hitotsu", _("single"))
    assert_equal("fr_hitotsu", n_("single", "plural", 1))
    assert_equal("fr_fukusu", n_("single", "plural", 2))
    assert_equal("fr_one", n_("one", "two", 1))
    assert_equal("fr_plural", n_("one", "two", 2))
    assert_equal("fr_one", _("one"))

    assert_equal("fr_hitotsu", n_("single", "not match", 1))
    assert_equal("fr_fukusu", n_("single", "not match", 2))
  end

  def test_Nn_
    GetText.locale = nil
    bindtextdomain("plural", "locale", "da")
    assert_equal(["one", "two"], Nn_("one", "two"))
  end

  def test_textdomain
    GetText.locale = nil
    Locale.set("ja_JP.eucJP")
    testlib = TestLib4.new
    assert_equal("japanese", testlib.test)
    assert_raises(GetText::NoboundTextDomainError) {
      GetText.textdomain("nodomainisdefined")
    }
    prefix = TextDomain::CONFIG_PREFIX
    default_locale_dirs = [
      "#{Config::CONFIG['datadir']}/locale/%{locale}/LC_MESSAGES/%{name}.mo",
      "#{Config::CONFIG['datadir'].gsub(/\/local/, "")}/locale/%{locale}/LC_MESSAGES/%{name}.mo",
      "#{prefix}/share/locale/%{locale}/LC_MESSAGES/%{name}.mo",
      "#{prefix}/local/share/locale/%{locale}/LC_MESSAGES/%{name}.mo"
    ].uniq
    assert_equal(default_locale_dirs, GetText::TextDomain::DEFAULT_LOCALE_PATHS)
    new_path = "/foo/%{locale}/%{name}.mo"
    GetText::TextDomain.add_default_locale_path(new_path)
    assert_equal([new_path] + default_locale_dirs, GetText::TextDomain::DEFAULT_LOCALE_PATHS)
  end

  def test_setlocale
    GetText.locale = nil
    bindtextdomain("test1", "locale")
    assert_equal("japanese", _("language"))
    setlocale("en")
    assert_equal("language", _("language"))
    setlocale("fr")
    assert_equal("french", _("language"))

    Locale.set "en"
    bindtextdomain("test1", "locale")
    assert_equal("language", _("language"))

    Locale.set "ja"
    bindtextdomain("test1", "locale")
    assert_equal("japanese", _("language"))

    # Confirm to set Locale::Object.
    loc = Locale::Object.new("ja_JP.eucJP")
    assert_equal(loc, GetText.locale = loc)
    assert_equal(Locale::Object, GetText.locale.class)
  end
end
