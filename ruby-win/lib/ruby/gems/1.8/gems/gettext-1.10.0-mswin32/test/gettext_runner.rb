require 'test/unit/testsuite'
require 'gettext_test'
require 'gettext_test_parser'
require 'gettext_test_string'
require 'gettext_test_locale'

class GetTextTest
   def suite
     s = Test::Unit::TestSuite.new
     s << TestGetText.suite
     s << TestGetTextParser.suite
     s << TestGetTextString.suite
     s << TestLocale.suite
     s
   end
end

if RUBY_VERSION >= '1.8.3'
  Test::Unit::AutoRunner.run
else
  Test::Unit::AutoRunner.run(GetTextTest)
end
