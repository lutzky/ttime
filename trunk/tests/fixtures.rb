# = Fixture support for testing
#
# Putting our actual fixture data with the tests is pretty bulky, especially
# if we have a lot of data. Here's how you use this fixtures addon:
#
# Create a file in +tests/fixtures+, call it +my_numbers.yml+. Have it
# contain the following YAML:
#
#   five: 5
#   four: 4
# 
# Then, within your test case, request this fixture. (You can request
# multiple fixtures on the same line or on separate lines)
#
#   class MyTest << Test::Unit::TestCase
#     fixture :my_numbers
#
# After you've done this, your fixtures will be available from your
# tests like this:
#
#   def test_something
#     assert_equal 4, my_numbers(:four)
#   end
#
# *Important*:: Fixtures get reloaded for each test separately (I might
#               add an option to change this later on), and only get loaded if
#               they're used.
#
# *Important*:: This currently overrides any current +setup+ method you
#               might have for your test class. Sorry :(

class Test::Unit::TestCase
  FIXTURES_PATH = Pathname.new "tests/fixtures"

  class << self
    def fixture(*fixture_syms)
      fixture_syms.each do |fixture_sym|
        class_eval do
          @@fixtures ||= []
          @@fixtures << fixture_sym

          define_method(fixture_sym) do |*args|
            if instance_variable_get("@#{fixture_sym}").nil?
              y = (FIXTURES_PATH + "#{fixture_sym.to_s}.yml").open do |yf|
                YAML.load yf
              end

              instance_variable_set("@#{fixture_sym}", y)
            end

            var = instance_variable_get("@#{fixture_sym}")

            if args.empty?
              var
            else
              var[args[0].to_s]
            end
          end

          define_method(:setup) do
            @@fixtures.each do |fixture_sym|
              instance_variable_set("@#{fixture_sym}", nil)
            end
          end
        end
      end
    end
  end
end


