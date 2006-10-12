class Test::Unit::TestCase
  FIXTURES_PATH = Pathname.new "tests/fixtures"

  class << self
    def fixture(fixture_sym)
      class_eval do
        @@fixtures ||= []
        @@fixtures << fixture_sym

        define_method(fixture_sym) do |key_sym|
          var = instance_variable_get("@#{fixture_sym}")
          return var[key_sym.to_s] if var

          y = (FIXTURES_PATH + "#{fixture_sym.to_s}.yml").open do |yf|
            YAML.load yf
          end

          instance_variable_set("@#{fixture_sym}", y)

          method(fixture_sym).call(key_sym)
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


