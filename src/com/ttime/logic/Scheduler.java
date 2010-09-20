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

    Scheduler(Collection<Course> courses, Collection<Constraint> constraints,
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
    }

    public List<Schedule> findSchedules() {
        LinkedList<Schedule> results = new LinkedList<Schedule>();

        Collections.sort(results, combinedComparator);
        return results;
    }

    private void buildSchedules(Schedule subSchedule,
            LinkedList<Schedule> results,
            List<Course> courses) {
        Course c = courses.get(0);
        List<Course> remainingCourses = courses.subList(1, courses.size());

        Schedule add = new Schedule(subSchedule);

        // TODO implement selective Cartesian product of all getGroupTypes

    }
}
