package com.ttime.logic;

import java.util.HashSet;
import java.util.Set;

public class Group {
    public enum Type {
        LECTURE, TUTORIAL, LAB, OTHER
    }

    int number;
    Type type;
    Set<Event> events;
    String lecturer;

    public String getLecturer() {
        return lecturer;
    }

    public void setLecturer(String lecturer) {
        this.lecturer = lecturer;
    }

    public Group(int number, Type type) {
        this.number = number;
        this.type = type;
        this.events = new HashSet<Event>();
        this.lecturer = null;
    }

    public Set<Event> getEvents() {
        return this.events;
    }
}
