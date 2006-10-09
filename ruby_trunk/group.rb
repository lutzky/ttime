#!/usr/bin/env ruby


class Group

  attr_reader :day, :start_time, :end_time, :room, :lecturer, :type
  attr_writer :day, :start_time, :end_time, :room, :lecturer, :type
  def type_is? (x)
    type == x.to_sym
  end

end
