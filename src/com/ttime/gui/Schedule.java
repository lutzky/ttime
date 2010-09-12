package com.ttime.gui;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Font;
import java.awt.GradientPaint;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Rectangle;
import java.awt.RenderingHints;
import java.awt.geom.RectangularShape;
import java.awt.geom.RoundRectangle2D;
import java.util.ArrayList;
import java.util.Collection;
import java.util.LinkedList;

import javax.swing.JPanel;

import com.ttime.TTime;
import com.ttime.logic.Event;

public class Schedule extends JPanel {
    int days;
    int startTime;
    int endTime;

    Graphics2D g;

    Collection<Event> events = new ArrayList<Event>();

    int getDurationHeight(int seconds) {
        return seconds * g.getClipBounds().height / (endTime - startTime);
    }

    Schedule() {
        super();

        Event[] initEvents = {
                new Event(4, TTime.seconds("9:30"), TTime.seconds("10:30"),
                        "אולמן 305"),
                new Event(4, TTime.seconds("10:00"), TTime.seconds("10:50"),
                        "אולמן 305"),
                new Event(4, TTime.seconds("10:40"), TTime.seconds("12:00"),
                        "אולמן 305"),
                new Event(3, TTime.seconds("10:30"), TTime.seconds("12:30"),
                        "אולמן 305"),
                new Event(3, TTime.seconds("12:30"), TTime.seconds("13:30"),
                        "אולמן 305"),
                new Event(2, TTime.seconds("10:30"), TTime.seconds("12:30"),
                        "אולמן 305"),
                new Event(1, TTime.seconds("10:30"), TTime.seconds("12:30"),
                        "אולמן 305"),
                new Event(1, TTime.seconds("11:30"), TTime.seconds("13:30"),
                        "אולמן 305"),
                new Event(1, TTime.seconds("12:30"), TTime.seconds("14:30"),
                        "אולמן 305"),
                new Event(1, TTime.seconds("13:00"), TTime.seconds("15:30"),
                        "אולמן 305"),
                new Event(2, TTime.seconds("11:30"), TTime.seconds("13:30"),
                        "אולמן 305"), };

        events.clear();
        for (Event e : initEvents) {
            events.add(e);
        }
    }

    void drawEvent(Event e, int numLayers, int layer) {
        // Always keep in mind one extra "day" for the hours column.
        // Also, keep in mind that we're drawing on the left of the day column,
        // which usually means we subtract one more "day". Similarly, we have
        // one extra "hour" for the days row.

        int width = (g.getClipBounds().width / (days + 1) / numLayers);
        int daysX = days - e.getDay() - 1;
        int dayWidth = g.getClipBounds().width / (days + 1);

        int x = daysX * dayWidth + width * (numLayers - layer - 1);

        int y = getDurationHeight(e.getStartTime() - startTime + 3600);

        int height = getDurationHeight(e.getEndTime() - e.getStartTime());

        g.setStroke(new BasicStroke(2.0f));

        g.setPaint(new GradientPaint(x, y, new Color(0xffcccc), x + width, y
                + height, new Color(0xffffff)));

        RectangularShape r = new RoundRectangle2D.Double(x, y, width, height,
                15, 15);

        g.fill(r);
        g.setColor(Color.BLACK);
        g.draw(r);
    }

    void computeTimeLimits(int earliestStart, int latestFinish) {
        // We work on an hour-long, offset-by-30-minute grid, so we want to
        // start and end on the half-hour.

        startTime = 3600 * (earliestStart / 3600) + 1800;

        if (earliestStart % 3600 < 1800) {
            startTime -= 3600;
        }

        endTime = 3600 * (latestFinish / 3600) + 1800;

        if (latestFinish % 3600 > 1800) {
            // Ends after the half-hour - add one hour
            endTime += 3600;
        }
    }

    @Override
    synchronized protected void paintComponent(Graphics g1) {
        super.paintComponent(g1);

        days = 5; // TODO this should be based on the events we actually get,
        computeTimeLimits(8 * 3600, 18 * 3600);
        // but a minimum of 5
        g = (Graphics2D) g1;

        g.setFont(new Font("Dialog", Font.BOLD, 40));

        Rectangle bounds = g.getClipBounds();

        RenderingHints rh = new RenderingHints(RenderingHints.KEY_ANTIALIASING,
                RenderingHints.VALUE_ANTIALIAS_ON);

        g.setRenderingHints(rh);

        g.setStroke(new BasicStroke(1.0f));

        for (int i = 0; i < (endTime - startTime) / 3600; i++) {
            int y = getDurationHeight(3600 * i);
            g.drawLine(0, y, (int) bounds.getWidth(), y);
        }

        drawEvents();
    }

    synchronized private void drawEvents() {
        /*
         * TODO optimize for speed by sorting by start time
         */

        // Credit: Basic algorithm by Boaz Goldstein

        LinkedList<Event> remainingEvents = new LinkedList<Event>(events);

        while (!remainingEvents.isEmpty()) {
            Event kernel = null;
            LinkedList<Event> currentCollidingSet = new LinkedList<Event>();
            boolean fixedPointReached = false;

            kernel = remainingEvents.getFirst();

            remainingEvents.remove(kernel);
            currentCollidingSet.add(kernel);

            fixedPoint:
            while (!fixedPointReached) {
                fixedPointReached = true;
                for (Event e : remainingEvents) {
                    if (e.collides(currentCollidingSet)) {
                        remainingEvents.remove(e);
                        currentCollidingSet.add(e);
                        fixedPointReached = false;
                        continue fixedPoint;
                    }
                }
            }

            LinkedList<LinkedList<Event>> layers = new LinkedList<LinkedList<Event>>();

            while (!currentCollidingSet.isEmpty()) {
                fixedPointReached = false;
                LinkedList<Event> layer = new LinkedList<Event>();
                layer.add(currentCollidingSet.getFirst());
                currentCollidingSet.removeFirst();

                fixedPoint: while (!fixedPointReached) {
                    fixedPointReached = true;

                    for (Event e : currentCollidingSet) {
                        if (!e.collides(layer)) {
                            layer.add(e);
                            currentCollidingSet.remove(e);
                            fixedPointReached = false;
                            continue fixedPoint;
                        }
                    }
                }
                layers.add(layer);
            }

            int i = 0;
            for (LinkedList<Event> layer : layers) {
                for (Event e : layer) {
                    drawEvent(e, layers.size(), i);
                }
                i += 1;
            }
        }
    }
}
