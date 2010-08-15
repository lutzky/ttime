package com.ttime.logic;

import java.util.HashSet;
import java.util.Set;

public class Course {
    int number;
    String name;
    float points;
    String lecturerInCharge;
    
    Set<Group> groups;
    
    // TODO These need to be represented as dates
    String firstTestDate;
    String secondTestDate;
    
    public Set<Group> getGroups() {
        return this.groups;
    }
    
    public String getLecturerInCharge() {
        return lecturerInCharge;
    }

    public void setLecturerInCharge(String lecturerInCharge) {
        this.lecturerInCharge = lecturerInCharge;
    }

    public String getFirstTestDate() {
        return firstTestDate;
    }

    public void setFirstTestDate(String firstTestDate) {
        this.firstTestDate = firstTestDate;
    }

    public String getSecondTestDate() {
        return secondTestDate;
    }

    public void setSecondTestDate(String secondTestDate) {
        this.secondTestDate = secondTestDate;
    }

    public float getPoints() {
        return points;
    }

    public void setPoints(float points) {
        this.points = points;
    }

    public Course(int number, String name) {
        this.number = number;
        this.name = name;
        this.groups = new HashSet<Group>();
    }
}