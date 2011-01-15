package com.ttime.logic;

import java.util.Collection;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedList;
import java.util.List;
import java.util.Set;

import com.ttime.logic.Group.Type;

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
            {String.valueOf(points), "נקודות אקדמיות"},
            {lecturerInCharge, "מרצה אחראי"},
            {firstTestDate, "מועד א'"},
            {secondTestDate, "מועד ב'"}
        };

        for (String[] pair : items) {
            if (pair[0] != null) {
                sb.append(String.format("<div><b>%s:</b> %s", pair[1], pair[0]));
            }
        }

        // TODO Add group and event details

        return sb.toString();
    }

    public Collection<Group> getGroupsByType(Group.Type t) {
        HashSet<Group> result = new HashSet<Group>();
        for (Group g : groups) {
            if (g.getType() == t) {
                result.add(g);
            }
        }
        return result;
    }
    private HashMap<Type, LinkedList<Group>> groupsByType = null;

    private HashMap<Type, LinkedList<Group>> getGroupsByType() {
        if (groupsByType != null) {
            return groupsByType;
        }

        groupsByType = new HashMap<Group.Type, LinkedList<Group>>();
        for (Group g : this.groups) {
            if (!groupsByType.containsKey(g.getType())) {
                groupsByType.put(g.getType(), new LinkedList<Group>());
            }
            groupsByType.get(g.getType()).add(g);
        }

        return groupsByType;
    }

    public List<Schedule> getSchedulingOptions() {
        LinkedList<Schedule> schedulingOptions = new LinkedList<Schedule>();
        addPartialSchedulingOptions(new Schedule(), schedulingOptions,
                new LinkedList<Group.Type>(getGroupsByType().keySet()));
        return schedulingOptions;
    }

    private void addPartialSchedulingOptions(Schedule subSchedule,
            LinkedList<Schedule> results, List<Group.Type> types) {
        if (types.isEmpty()) {
            results.add(subSchedule);
            return;
        }

        Group.Type currentType = types.get(0);
        List<Group.Type> remainingTypes = types.subList(1, types.size());

        for (Group g : getGroupsByType(currentType)) {
            Schedule amendedSchedule = (Schedule) subSchedule.clone();
            amendedSchedule.addAll(g.getEvents());
            addPartialSchedulingOptions(amendedSchedule, results, remainingTypes);
        }
    }
}
