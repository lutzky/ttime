require 'date'

require 'logic/group'
require 'logic/shared'
require 'logic/course'

module TTime
  module Logic
    class SportsCourse < Course

      def initialize(header,line)
        @academic_points = 1.0
        @first_test_date = '-'
        @second_test_date = '-'

        # pardon my rude perling, but
        arr =  /\|\s+(\d\d\d\d\d\d)\s+([א-תףץךןם0-9()\/+#,.\-"'_: ]+?) *\|\n\| +שעות הוראה בשבוע\: +( *[א-ת].+?[0-9]+)+ +נק: (\d\.?\d) +\|/.match(header)
        @number = arr[1].reverse
        get_name = /\|\s+(\d\d) (.*?)\s+([אבגדהו]'[0-9].*\|/.match(line)
        @name = arr[2].strip.single_space + " - " + get_name[2].strip.single_space
      end
    end

  end
end


