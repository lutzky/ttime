if RUBY_VERSION < "1.9"
  require 'iconv'

  $KCODE = 'u'
  require 'jcode'

  $utf8_converter = Iconv.new('utf-8', 'cp862')
  $cp862_converter = Iconv.new('cp862', 'utf-8')

  module Encoding
    IBM862 = "ibm862"
  end
  class String
    def encode encoding
      if encoding == "utf-8"
        return $utf8_converter.iconv self
      elsif encoding = Encoding::IBM862
        return $cp862_converter.iconv self
      end
    end
  end
end
