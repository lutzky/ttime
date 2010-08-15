package com.ttime.logic;

import java.util.HashSet;
import java.util.Set;

public class Faculty {
    String name;
    Set<Course> courses;

    public Set<Course> getCourses() {
        return courses;
    }

    public Faculty(String name) {
        this.name = name;
        this.courses = new HashSet<Course>();
    }
}
