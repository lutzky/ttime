package com.ttime.logic;

import java.util.Collection;

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

    public boolean collides(Event rhs) {
        // Momentary "collisions" do not count, we always assume an implicit
        // ten-minute break at the end of the lesson. Sadly, lecturers don't
        // always give it...
        return this.day == rhs.day
                && !(this.endTime <= rhs.startTime || this.startTime >= rhs.endTime);
    }

    public int getDay() {
        return day;
    }

    public int getStartTime() {
        return startTime;
    }

    public int getEndTime() {
        return endTime;
    }

    public String getPlace() {
        return place;
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

    /**
     * Check whether an event collides with any event in a collection of events.
     *
     * @param c
     *            A collection of events
     * @param e
     *            A single event
     * @return true if e collides with any of the events in c, false otherwise.
     */
    public static boolean collides(Collection<Event> c, Event e) {
        for (Event ce : c) {
            if (ce.collides(e)) {
                return true;
            }
        }
        return false;
    }
}