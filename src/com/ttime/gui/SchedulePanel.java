package com.ttime.gui;

import java.awt.BorderLayout;
import java.util.Collection;
import java.util.LinkedList;
import java.util.List;

import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JSpinner;
import javax.swing.SpinnerNumberModel;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;

import com.ttime.logic.Schedule;

public class SchedulePanel extends JPanel {

    final ScheduleView scheduleView = new ScheduleView();
    List<Schedule> schedules = new LinkedList<Schedule>();
    final JSpinner scheduleSpinner = new JSpinner(new SpinnerNumberModel(0, 0,
            0, 0));
    final JLabel ofHowMany = new JLabel("of 0");

    public SchedulePanel() {
        super();
        this.setLayout(new BorderLayout());

        JPanel scheduleSelector = new JPanel();

        scheduleSpinner.addChangeListener(new ChangeListener() {

            @Override
            public void stateChanged(ChangeEvent arg0) {
                scheduleView.setEvents(schedules.get((Integer) scheduleSpinner.getModel().getValue() - 1));
            }
        });

        scheduleSelector.add(scheduleSpinner);
        scheduleSelector.add(ofHowMany);
        this.add(scheduleSelector, BorderLayout.NORTH);

        this.add(scheduleView, BorderLayout.CENTER);
    }

    public void setSchedules(Collection<Schedule> schedules) {
        if (schedules.isEmpty()) {
            return;
        }
        this.schedules = new LinkedList<Schedule>(schedules);
        scheduleSpinner.setModel(new SpinnerNumberModel(1, 1, schedules.size(),
                1));
        ofHowMany.setText("of " + schedules.size());
        scheduleView.setEvents(this.schedules.get(0));

    }
}
