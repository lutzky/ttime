package com.ttime.gui;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Font;
import java.awt.GradientPaint;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Rectangle;
import java.awt.RenderingHints;
import java.awt.font.FontRenderContext;
import java.awt.font.LineBreakMeasurer;
import java.awt.font.TextAttribute;
import java.awt.font.TextLayout;
import java.awt.geom.RectangularShape;
import java.awt.geom.RoundRectangle2D;
import java.text.AttributedString;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.LinkedList;
import java.util.ListIterator;

import javax.swing.JComponent;

import com.ttime.Utils;
import com.ttime.logic.Event;

@SuppressWarnings("serial")
public class ScheduleView extends JComponent {
    protected static final float SMALL_MARGIN = 5;
    protected static final String[] DAY_NAMES = { "ראשון", "שני", "שלישי",
        "רביעי", "חמישי", "שישי", "שבת" };

    int days;
    int startTime;
    int endTime;

    Graphics2D g;

    Collection<Event> events = new ArrayList<Event>();

    int getDurationHeight(int seconds) {
        return seconds * g.getClipBounds().height / (endTime - startTime);
    }

    void setEvents(Collection<Event> events) {
        this.events.clear();
        this.events.addAll(events);
        this.repaint();
    }

    void drawEvent(Event e, int numLayers, int layer) {
        // Always keep in mind one extra "day" for the hours column.
        // Also, keep in mind that we're drawing on the left of the day column,
        // which usually means we subtract one more "day". Similarly, we have
        // one extra "hour" for the days row.

        // TODO Avoid magic numbers for rounded corners, padding

        // Create a new graphics context so our clipping doesn't last
        Graphics2D g = (Graphics2D) this.g.create();

        float width = (g.getClipBounds().width / (days + 1) / numLayers);
        float daysX = days - e.getDay() - 1;

        float x = daysX * dayWidth() + width * (numLayers - layer - 1);

        float y = getDurationHeight(e.getStartTime() - startTime + 3600);

        float height = getDurationHeight(e.getEndTime() - e.getStartTime());

        g.setStroke(new BasicStroke(2.0f));

        g.setPaint(new GradientPaint(x, y, new Color(0xffcccc), x + width, y
                + height, new Color(0xffffff)));

        RectangularShape r = new RoundRectangle2D.Double(x, y, width, height,
                15, 15);

        g.fill(r);
        g.setColor(Color.BLACK);
        g.draw(r);

        g.setClip(r);
        FontRenderContext frc = g.getFontRenderContext();

        y += 5;

        AttributedString attributedTitle = new AttributedString(e.getCourse()
                .toString());
        attributedTitle.addAttribute(TextAttribute.WEIGHT,
                TextAttribute.WEIGHT_BOLD);
        LineBreakMeasurer lbmTitle = new LineBreakMeasurer(attributedTitle
                .getIterator(), frc);

        while (lbmTitle.getPosition() < attributedTitle.getIterator()
                .getEndIndex()) {
            TextLayout tl = lbmTitle.nextLayout(width - 10);
            tl.draw(g, x + width - tl.getAdvance() - 5, y + tl.getAscent());
            y += tl.getAscent();
        }

        if (e.getPlace() != null) {
            AttributedString attributedPlace = new AttributedString(e
                    .getPlace());
            attributedTitle.addAttribute(TextAttribute.POSTURE,
                    TextAttribute.POSTURE_OBLIQUE);
            LineBreakMeasurer lbmPlace = new LineBreakMeasurer(attributedPlace
                    .getIterator(), frc);

            while (lbmPlace.getPosition() < attributedPlace.getIterator()
                    .getEndIndex()) {
                TextLayout tl = lbmPlace.nextLayout(width - 10);
                tl.draw(g, x + width - tl.getAdvance() - 5, y + tl.getAscent());
                y += tl.getAscent();
            }
        }
    }

    private float dayWidth() {
        return g.getClipBounds().width / (days + 1);
    }

    void computeTimeLimits() {
        // We work on an hour-long, offset-by-30-minute grid, so we want to
        // start and end on the half-hour.

        int earliestStart;
        int latestFinish;
        days = 5;

        if (events.isEmpty()) {
            earliestStart = 8 * 3600;
            latestFinish = 16 * 3600;
        } else {
            Event firstEvent = events.iterator().next();
            earliestStart = firstEvent.getStartTime();
            latestFinish = firstEvent.getEndTime();
        }

        for (Event e : events) {
            earliestStart = java.lang.Math.min(e.getStartTime(), earliestStart);
            latestFinish = java.lang.Math.max(e.getEndTime(), latestFinish);
            if (e.getDay() >= 5) {
                days = e.getDay();
            }
        }

        startTime = 3600 * (earliestStart / 3600) + 1800;

        if (earliestStart % 3600 < 1800) {
            startTime -= 3600;
        }

        endTime = 3600 * (latestFinish / 3600) + 1800;

        if (latestFinish % 3600 >= 1800) {
            // Ends after the half-hour - add one hour
            endTime += 3600;
        }
    }

    @Override
    synchronized protected void paintComponent(Graphics g1) {
        this.g = (Graphics2D) g1;
        computeTimeLimits();

        g.setFont(new Font("Dialog", Font.BOLD, 16));

        Rectangle bounds = g.getClipBounds();

        RenderingHints rh = new RenderingHints(RenderingHints.KEY_ANTIALIASING,
                RenderingHints.VALUE_ANTIALIAS_ON);

        g.setRenderingHints(rh);

        g.setStroke(new BasicStroke(1.0f));

        g.setFont(new Font("Dialog", Font.BOLD, 16));

        for (int i = 0; i < (endTime - startTime) / 3600; i++) {
            int y = getDurationHeight(3600 * i);
            g.drawLine(0, y, (int) bounds.getWidth(), y);

            if (i == 0) {
                // Don't draw the time under the first line, that's just the
                // top border.
                continue;
            }

            String lineTime = Utils.formatTime(startTime + (i - 1) * 3600);

            TextLayout tl = new TextLayout(lineTime, g.getFont(), g
                    .getFontRenderContext());

            g.drawString(lineTime, days * dayWidth() + SMALL_MARGIN, y
                    + tl.getAscent() + SMALL_MARGIN);
        }

        for (int i = 1; i <= days; i++) {
            // i goes from 1 to days - no left border is needed, but we have
            // an extra "day" as the hours list on the right.
            g.drawLine((int) (i * dayWidth()), 0, (int) (i * dayWidth()),
                    (int) bounds.getHeight());

            String dayName = DAY_NAMES[days - i];

            TextLayout tl = new TextLayout(dayName, g.getFont(), g
                    .getFontRenderContext());
            g.drawString(dayName, (i) * dayWidth() - tl.getAdvance()
                    - SMALL_MARGIN, tl.getAscent() + SMALL_MARGIN);
        }

        drawEvents();
    }

    synchronized private void drawEvents() {
        // Credit: Basic algorithm by Boaz Goldstein

        LinkedList<Event> remainingEvents = new LinkedList<Event>(events);
        Collections.sort(remainingEvents);

        while (!remainingEvents.isEmpty()) {
            LinkedList<Event> collidingEvents = new LinkedList<Event>();

            collidingEvents.add(remainingEvents.getFirst());
            remainingEvents.removeFirst();

            int collisionDay = collidingEvents.getFirst().getDay();
            int collisionEndTime = collidingEvents.getFirst().getEndTime();

            // Expand the collidingEvents set with all colliding events. Since
            // remainingEvents is sorted by startTime, we only need to check
            // equality of the day and that endTime falls within our collision
            // block.
            while (!remainingEvents.isEmpty()
                    && remainingEvents.getFirst().getDay() == collisionDay
                    && remainingEvents.getFirst().getStartTime() < collisionEndTime) {
                Event newCollider = remainingEvents.getFirst();
                collisionEndTime = Math.max(collisionEndTime, newCollider
                        .getEndTime());
                collidingEvents.add(newCollider);
                remainingEvents.removeFirst();
            }

            // collidingEvents need to be split into layers. We do this
            // greedily.

            LinkedList<LinkedList<Event>> layers = new LinkedList<LinkedList<Event>>();

            // collidingEvents is sorted, as it is a prefix of the sorted
            // remainingEvents.
            // We will create each layer by taking the first event in
            // collidingEvents,
            // all events which don't collide with it (they start later, we only
            // need
            // to check that they start after it ends), and removing all of
            // those events.

            while (!collidingEvents.isEmpty()) {
                LinkedList<Event> layer = new LinkedList<Event>();

                layer.add(collidingEvents.getFirst());
                int layerEnd = layer.getFirst().getEndTime();
                collidingEvents.removeFirst();

                ListIterator<Event> it = collidingEvents.listIterator();

                while (it.hasNext()) {
                    Event e = it.next();
                    if (e.getStartTime() >= layerEnd) {
                        // e does not collide.
                        it.remove();
                        layer.add(e);
                        layerEnd = e.getEndTime();
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
