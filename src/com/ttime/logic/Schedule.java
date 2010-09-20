package com.ttime.logic;

import java.util.LinkedList;

public class Schedule extends LinkedList<Event> {

    public Schedule(Schedule subSchedule) {
        super(subSchedule);
    }
}
