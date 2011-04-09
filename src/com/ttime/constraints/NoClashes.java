package com.ttime.constraints;

import java.util.Collections;

import com.ttime.logic.Event;
import com.ttime.logic.Schedule;

public class NoClashes implements Constraint {

	@Override
	public boolean accepts(Schedule s) {
		Schedule sorted = (Schedule) s.clone();
		Collections.sort(sorted);

		Event prev_e = null;

		for (Event e : sorted) {
			if ((prev_e != null) && (prev_e.collides(e))) {
				return false;
			}

			prev_e = e;
		}

		return true;
	}
}
