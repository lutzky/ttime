package com.ttime.constraints;

import java.util.Collection;
import java.util.LinkedList;

public class ConstraintManager {
    Collection<Constraint> constraints;

    private static ConstraintManager instance = null;

    private ConstraintManager() {
        // TODO Load settings, load only desired constraints.
        constraints = new LinkedList<Constraint>();
        constraints.add(new NoClashes());
    }

    public static ConstraintManager getInstance() {
        if (instance == null) {
            instance = new ConstraintManager();
        }

        return instance;
    }

    public Collection<Constraint> getConstraints() {
        return constraints;
    }

}
