package com.ttime.constraints;

import com.ttime.logic.Schedule;

public interface Constraint {
    boolean accepts(Schedule s);
}
