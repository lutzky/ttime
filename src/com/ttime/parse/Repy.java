package com.ttime.parse;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.LineNumberReader;
import java.nio.charset.Charset;
import java.text.ParseException;
import java.util.HashSet;
import java.util.NoSuchElementException;
import java.util.Set;
import java.util.logging.Logger;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import com.ttime.logic.Course;
import com.ttime.logic.Event;
import com.ttime.logic.Faculty;
import com.ttime.logic.Group;

public class Repy {
    static class Expressions {
        final static String FACULTY_SEPARATOR = "+==========================================+";

        final static String COURSE_SEPARATOR = "+------------------------------------------+";

        final static Pattern FACULTY_NAME = Pattern
                .compile("\\| מערכת שעות - (.*) +\\|");

        final static Pattern COURSE_NAME_NUMBER = Pattern
                .compile("\\| +(\\d{6}) +(.*?) *\\|");

        final static Pattern COURSE_HOURS_POINTS = Pattern
                .compile("\\| +שעות הוראה בשבוע:( *[א-ת].+?[0-9]+)* +נק: (\\d\\.?\\d) *\\|");

        final static Pattern LECTURER_IN_CHARGE = Pattern
                .compile("\\| מורה\\s+אחראי :(.*?) *\\|");

        final static Pattern GROUP_LECTURER = Pattern
                .compile("\\|\\s*מרצה\\s*:(.*?)\\s*\\|");

        final static Pattern FIRST_TEST_DATE = Pattern
                .compile("\\| מועד ראשון :(.*?) *\\|");

        final static Pattern SECOND_TEST_DATE = Pattern
                .compile("\\| מועד שני   :(.*?) *\\|");

        final static String REGISTRATION_BLANK = "|רישום                                     |";
        final static String BLANK = "|                                          |";

        final static Pattern GROUP = Pattern
                .compile("\\| *([0-9]*) *([א-ת]+) ?: ?(.*?) *\\|");

        final static String GROUP_TYPE_LECTURE = "הרצאה";
        final static String GROUP_TYPE_LAB = "מעבדה";
        final static String GROUP_TYPE_TUTORIAL = "תרגיל";

        final static Pattern EVENT_LINE = Pattern
                .compile("(.+)'(\\d+.\\d+) ?-(\\d+.\\d+) *(.*)");

        final static Pattern BLANK_WITH_DASH = Pattern.compile("^ *- *$");

        final static Pattern ANYTHING = Pattern.compile("\\| +(.*?) +\\|");
    }

    enum CourseParserState {
        START, THING, DETAILS
    }

    /**
     * Reverse a string (you'd think Java would have a method to do this...)
     *
     * @param s
     *            string to reverse
     * @return s, reversed.
     */
    static String reverse(String s) {
        return new StringBuffer(s).reverse().toString();
    }

    LineNumberReader REPY_file;
    String current_line;
    Faculty current_faculty;

    Set<Faculty> faculties;

    Logger log;

    public Repy(File filename) throws IOException, ParseException {
        log = Logger.getLogger("global");

        REPY_file = new LineNumberReader(new InputStreamReader(
                new FileInputStream(filename), Charset.forName("cp862")));
        System.out.printf("Constructing a Repy from %s\n", filename);

        faculties = new HashSet<Faculty>();

        while (readRepyLine() != null) {
            if (current_line.equals(Expressions.FACULTY_SEPARATOR)) {
                log.fine("Read faculty separator, parsing a faculty.");
                parse_a_faculty();
            }
        }

        System.out.println("Got the following faculties:");
        for (Faculty f : faculties) {
            System.out.println(f);
        }
    }

    String readRepyLine() throws IOException {
        current_line = reverse(REPY_file.readLine());
        log.finer(String.format("REPY: %s", current_line));
        return current_line;
    }

    void parse_a_faculty() throws IOException, ParseException {
        Course current_course;

        current_faculty = new Faculty(parse_faculty_header());

        log.fine(String.format(
                "Got a faculty, %s, and finished parsing its header.",
                current_faculty.getName()));

        try {
            while ((current_course = parse_a_course()) != null) {
                log.fine(String.format(
                        "Done parsing course <%d - %s>, adding it",
                        current_course.getNumber(), current_course.getName()));
                current_faculty.getCourses().add(current_course);
            }
        } catch (NoSuchElementException e) {
            // No more courses in this faculty. Next!
        }

        faculties.add(current_faculty);
    }

    Course parse_a_course() throws NoSuchElementException, IOException,
            ParseException {
        readRepyLine();

        if (current_line.isEmpty()) {
            throw new NoSuchElementException();
        }

        if (current_line.equals(Expressions.COURSE_SEPARATOR)) {
            log.fine("Throwing away extraneous course separator line.");
            readRepyLine();
        }

        Matcher m = Expressions.COURSE_NAME_NUMBER.matcher(current_line);

        if (!m.matches()) {
            throw parseError("Invalid course name-and-number line", current_line);
        }

        int course_number = Integer.valueOf(reverse(m.group(1)));
        String course_name = m.group(2).trim();

        Course course = new Course(course_number, course_name);

        log.fine(String.format("Got course name and number %d - %s", course_number, course_name));

        readRepyLine();

        m = Expressions.COURSE_HOURS_POINTS.matcher(current_line);

        if (!m.matches()) {
            throw parseError("Invalid course hours-and-points line");
        }

        course.setPoints(Float.valueOf(reverse(m.group(2))));

        log.fine(String.format("This is a %.1f-point course", course.getPoints()));

        if (!readRepyLine().equals(Expressions.COURSE_SEPARATOR)) {
            throw parseError("Expected course separator line", Expressions.COURSE_SEPARATOR);
        }

        log.fine("Got end of course header");

        log.fine(String.format("Starting to parse course %d - %s",
                course_number, course_name));

        log.fine("Setting state to START");
        CourseParserState state = CourseParserState.START;

        log.fine("Setting current lecture group number to 1");
        int current_lecture_group_number = 1;

        Group group = null;

        while (!readRepyLine().equals(Expressions.COURSE_SEPARATOR)) {
            log.fine(String.format("State is %s", state));
            switch (state) {
            case START:
                if (current_line.charAt(3) != '-') {
                    if ((m = Expressions.LECTURER_IN_CHARGE
                            .matcher(current_line)).matches()) {
                        log.fine("Got valid lecturer in charge");
                        course.setLecturerInCharge(m.group(1).trim()
                                .replaceAll("\\s+", " "));
                    } else if ((m = Expressions.FIRST_TEST_DATE
                            .matcher(current_line)).matches()) {
                        log.fine("Got valid first test date");
                        course.setFirstTestDate(m.group(1));
                    } else if ((m = Expressions.SECOND_TEST_DATE
                            .matcher(current_line)).matches()) {
                        log.fine("Got valid second test date");
                        course.setSecondTestDate(m.group(1));
                    } else if (current_line
                            .equals(Expressions.REGISTRATION_BLANK)
                            || current_line.equals(Expressions.BLANK)) {
                        log.fine("Changing state to THING");
                        state = CourseParserState.THING;
                    }
                }
                break;
            case THING:
                if (current_line.contains("----")) {
                    throw parseError("Unexpected line");
                } else if ((m = Expressions.GROUP.matcher(current_line))
                        .matches()) {

                    int group_number;

                    if (!m.group(1).isEmpty()) {
                        group_number = Integer.valueOf(m.group(1));
                        log.fine(String.format("Got group number %d", group_number));
                    } else {
                        group_number = 10 * current_lecture_group_number;
                        current_lecture_group_number += 1;
                        log.fine(String.format("Got no group number, guessing by count to %d", group_number));
                    }

                    group = new Group(group_number,
                            parse_group_type(m.group(2)));

                    Event e = parse_event_line(m.group(3));

                    if (e == null) {
                        log.fine("Blank event line, ignoring.");
                    }

                    log.fine("Got a valid event line.");

                    group.getEvents().add(e);

                    log.fine("Setting state to DETAILS.");

                    state = CourseParserState.DETAILS;
                }
            case DETAILS:
                if (!current_line.contains(":")) {
                    if (current_line.equals(Expressions.BLANK)
                            || current_line.contains("++++++")
                            || current_line.contains("----")) {
                        log.fine("Got an end-of-group marker.");
                        if ((group != null) && (!group.getEvents().isEmpty())) {
                            course.getGroups().add(group);
                            group = null;
                            log.fine("Group is non-empty, adding it.");
                        }
                        else {
                            log.fine("No or empty group to add, not adding");
                        }

                        log.fine("Setting state to THING.");
                        state = CourseParserState.THING;
                    } else {
                        m = Expressions.ANYTHING.matcher(current_line);
                        if (m.matches()) {
                            group.getEvents().add(parse_event_line(m.group(1)));
                            log.fine("Added a valid event line.");
                        }
                    }
                } else if ((m = Expressions.GROUP_LECTURER
                        .matcher(current_line)).matches()) {
                    group.setLecturer(m.group(1).trim());
                    log.fine("Got a valid group lecturer.");
                }
            }
        }

        if (state == CourseParserState.DETAILS) {
            if (!group.getEvents().isEmpty()) {
                course.getGroups().add(group);
            }
        }

        return course;
    }

    Event parse_event_line(String event_line) throws ParseException {
        if (Expressions.BLANK_WITH_DASH.matcher(event_line).matches()) {
            return null;
        }

        Character day_letter;
        Matcher m = Expressions.EVENT_LINE.matcher(event_line);

        if (!m.matches()) {
            throw parseError("Could not figure out the event line", event_line);
        }

        if (m.group(1).length() != 1) {
            throw parseError("Invalid day name", m.group(1));
        } else {
            day_letter = m.group(1).charAt(0);
        }

        return new Event(day_letter_to_number(day_letter), Integer
                .valueOf(reverse(m.group(3)).replaceAll("\\.", "")), Integer
                .valueOf(reverse(m.group(2)).replaceAll("\\.", "")),
                place_fix(m.group(4)));
    }

    int day_letter_to_number(Character day_letter) {
        return (day_letter - 'א') + 1;
    }

    /**
     * Places are halfway reversed in REPY: We treat all lines as reversed to
     * fix the Hebrew, but then we get reversed numbers.
     *
     * @param s
     *            - A "place" string in the form "PLACE\s+REVERSENUMBER"
     * @return "PLACE NUMBER"
     */
    String place_fix(String s) {
        log.fine(String.format("Running place_fix(\"%s\")", s));
        if (s.trim().isEmpty()) {
            return null;
        }
        String[] bits;
        StringBuilder sb = new StringBuilder();
        bits = s.split("\\s+");
        sb.append(bits[0]);
        if (bits.length > 1) {
            sb.append(reverse(bits[1]));
        }
        return sb.toString();
    }

    Group.Type parse_group_type(String s) {
        if (s.equals(Expressions.GROUP_TYPE_LAB)) {
            return Group.Type.LAB;
        } else if (s.equals(Expressions.GROUP_TYPE_LECTURE)) {
            return Group.Type.LECTURE;
        } else if (s.equals(Expressions.GROUP_TYPE_TUTORIAL)) {
            return Group.Type.TUTORIAL;
        } else {
            return Group.Type.OTHER;
        }
    }

    String parse_faculty_header() throws IOException, ParseException {
        String name_line = readRepyLine();
        String faculty_name;
        Matcher m = Expressions.FACULTY_NAME.matcher(name_line);

        if (m.matches()) {
            faculty_name = m.group(1).trim();
        } else {
            throw parseError("Invalid faculty name line");
        }

        // Read unused line describing the current semester
        readRepyLine();

        if (!readRepyLine().equals(Expressions.FACULTY_SEPARATOR)) {
            throw parseError("Expected end of faculty header");
        }

        return faculty_name;
    }

    ParseException parseError(String s) throws ParseException {
        return parseError(s, current_line);
    }

    ParseException parseError(String s, String unexpected_bit)
            throws ParseException {
        return new ParseException(String.format("%s: %s", s, unexpected_bit),
                REPY_file.getLineNumber());
    }
}
