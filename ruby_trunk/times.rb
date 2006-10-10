class Day 
  @@name_to_day = {
    'sun' => 1,
    'mon' => 2,
    'tue' => 3,
    'wed' => 4,
    'thr' => 5,
    'fri' => 6, 
    'א' => 1,
    'ב' => 2,
    'ג' => 3,
    'ד' => 4,
    'ה' => 5,
    'ו' => 6 

  }
  def initialize(x)
    if x.is_a? Integer
      @day = x;
    else
      puts "its #{x} day\n"
      @day = @@name_to_day[x]
    end
  end
end

class Hour
  def initialize(_hour)
    if _hour.is_a? Integer
      @hour = _hour
    else
      @hour = /(\d\d?)(:|\.)\d\d/.match(_hour)[1]
    end
  end
end
