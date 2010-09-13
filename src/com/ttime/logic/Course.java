package com.ttime.logic;

import java.util.HashSet;
import java.util.Set;

public class Course implements Comparable<Course> {
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

    public int getNumber() {
        return this.number;
    }

    public String getName() {
        return this.name;
    }

    @Override
    public String toString() {
        return String.format("%d %s", this.number, this.name);
    }

    @Override
    public int compareTo(Course rhs) {
        return new Integer(number).compareTo(rhs.getNumber());
    }

    public String getHtmlInfo() {
        StringBuilder sb = new StringBuilder();

        sb.append(String.format("<h1>[%d] %s</h1>", number, name));

        String[][] items = {
                { String.valueOf(points), "נקודות אקדמיות" },
                { lecturerInCharge, "מרצה אחראי" },
                { firstTestDate, "מועד א'" },
                { secondTestDate, "מועד ב'" }
        };

        for (String[] pair : items) {
            if (pair[0] != null) {
                sb.append(String.format("<div><b>%s:</b> %s", pair[1], pair[0]));
            }
        }

        // TODO Add group and event details

        return sb.toString();
    }
}