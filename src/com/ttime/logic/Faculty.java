package com.ttime.logic;

import java.util.HashSet;
import java.util.Set;

public class Faculty implements Comparable<Faculty> {
    String name;
    Set<Course> courses;

    public Set<Course> getCourses() {
        return courses;
    }

    public Faculty(String name) {
        this.name = name;
        this.courses = new HashSet<Course>();
    }

    public String getName() {
        return this.name;
    }

    @Override
    public String toString() {
        return this.name;
    }

    @Override
    public int compareTo(Faculty o) {
        return name.compareTo(o.getName());
    }
}
