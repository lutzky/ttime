package com.ttime.logic;

import java.util.Collection;
import java.util.Collections;
import java.util.Comparator;
import java.util.LinkedList;
import java.util.List;

public class Scheduler {

    private final LinkedList<Course> courses;
    private final List<Comparator<Schedule>> comparators;
    private final Collection<Constraint> constraints;
    private final Comparator<Schedule> combinedComparator;
    private final Constraint combinedConstraint;

    public Scheduler(Collection<Course> courses,
            Collection<Constraint> constraints,
            List<Comparator<Schedule>> comparators) {
        this.courses = new LinkedList<Course>(courses);
        this.constraints = constraints;
        this.comparators = comparators;

        combinedComparator = new Comparator<Schedule>() {

            @Override
            public int compare(Schedule o1, Schedule o2) {
                for (Comparator<Schedule> c : Scheduler.this.comparators) {
                    int comparisonResult = c.compare(o1, o2);
                    if (comparisonResult != 0) {
                        return comparisonResult;
                    }
                }
                return 0;
            }
        };

        combinedConstraint = new Constraint() {
            @Override
            public boolean accepts(Schedule s) {
                for (Constraint c : Scheduler.this.constraints) {
                    if (!c.accepts(s)) {
                        return false;
                    }
                }
                return true;
            }
        };
    }

    public List<Schedule> findSchedules() {
        LinkedList<Schedule> results = new LinkedList<Schedule>();

        buildSchedules(new Schedule(), results, courses);

        Collections.sort(results, combinedComparator);
        return results;
    }

    private void buildSchedules(Schedule subSchedule,
            LinkedList<Schedule> results,
            List<Course> courses) {

        if (!combinedConstraint.accepts(subSchedule)) {
            return;
        }

        if (courses.isEmpty()) {
            results.add(subSchedule);
            return;
        }

        Course c = courses.get(0);
        List<Course> remainingCourses = courses.subList(1, courses.size());

        for (Schedule schedulingOption : c.getSchedulingOptions()) {
            Schedule amendedSchedule = (Schedule) subSchedule.clone();
            amendedSchedule.addAll(schedulingOption);
            buildSchedules(amendedSchedule, results, remainingCourses);
        }
    }
}
