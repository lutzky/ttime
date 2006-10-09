#!/usr/bin/env ruby



class Group
  attr_reader :number, :lecturer, :type, :events
  attr_writer :number, :lecturer, :type, :events
  HebToType = {
    "הרצאה" => :lecture,
    "מעבדה" => :lab,
    "קבוצה" => :set,
    "תרגיל" => :tutorial
  }

  def type_is? (x)
    type == x.to_sym
  end

  def heb_type=(x)
    @type = HebToType[x]
  end

  def add_hours(x)
    @events = @events.to_a
    @events << x # FIXME: parse
  end

  def set_from_heb(x,y)
  end
end
