package com.ttime.logic;

public class Event {
    int day;

    /**
     * Stored as seconds since midnight
     */
    int startTime;

    /**
     * Stored as seconds since midnight
     */
    int endTime;
    String place;

    public Event(int day, int startTime, int endTime, String place) {
        this.day = day;
        this.startTime = startTime;
        this.endTime = endTime;
        this.place = place;
    }

    @Override
    public String toString() {
        return String.format("<Event day=%d startTime~\"%02d:%02d\" endTime~\"%02d:%02d\" place=\"%s\">",
                this.day,
                this.startTime / 3600,
                (this.startTime / 60) % 60,
                this.endTime / 3600,
                (this.endTime / 60) % 60,
                this.place);
    }
}