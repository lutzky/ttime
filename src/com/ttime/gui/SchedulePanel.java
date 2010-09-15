package com.ttime.gui;

import java.awt.Graphics;
import java.awt.Graphics2D;

import javax.swing.JPanel;

public class SchedulePanel extends JPanel {
    Schedule schedule = new Schedule();

    @Override
    synchronized protected void paintComponent(Graphics g) {
        schedule.paint((Graphics2D) g);
    }

}
