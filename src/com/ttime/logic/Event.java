package com.ttime.logic;

public class Event {
    int day;
    int startTime;
    int endTime;
    String place;
    
    public Event(int day, int startTime, int endTime, String place) {
        this.day = day;
        this.startTime = startTime;
        this.endTime = endTime;
        this.place = place;
    }
}