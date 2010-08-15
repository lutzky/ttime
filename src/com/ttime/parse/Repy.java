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
                .compile("\\| מורה אחראי :(.*?) *\\|");

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

    public Repy(File filename) throws IOException, ParseException {
        REPY_file = new LineNumberReader(new InputStreamReader(
                new FileInputStream(filename), Charset.forName("cp862")));
        System.out.printf("Constructing a Repy from %s\n", filename);

        faculties = new HashSet<Faculty>();

        while ((current_line = REPY_file.readLine()) != null) {
            if (current_line.equals(Expressions.FACULTY_SEPARATOR)) {
                parse_a_faculty();
            }
        }

        System.out.println("Got the following faculties:");
        for (Faculty f : faculties) {
            System.out.println(f);
        }
    }

    void parse_a_faculty() throws IOException, ParseException {
        Course current_course;

        current_faculty = new Faculty(parse_faculty_header());

        try {
            while ((current_course = parse_a_course()) != null) {
                current_faculty.getCourses().add(current_course);
            }
        } catch (NoSuchElementException e) {
            // No more courses in this faculty. Next!
        }

        faculties.add(current_faculty);
    }

    Course parse_a_course() throws NoSuchElementException, IOException,
            ParseException {
        current_line = REPY_file.readLine();

        if (current_line.isEmpty()) {
            throw new NoSuchElementException();
        }
        if (!current_line.equals(Expressions.COURSE_SEPARATOR)) {
            throw parseError("Expected course header");
        }

        current_line = reverse(REPY_file.readLine());
        Matcher m = Expressions.COURSE_NAME_NUMBER.matcher(current_line);

        if (!m.matches()) {
            throw parseError(String.format(
                    "Invalid course name-and-number line: %s", current_line));
        }

        int course_number = Integer.valueOf(reverse(m.group(1)));
        String course_name = m.group(2).trim();

        Course course = new Course(course_number, course_name);

        current_line = reverse(REPY_file.readLine());
        m = Expressions.COURSE_HOURS_POINTS.matcher(current_line);

        if (!m.matches()) {
            throw parseError("Invalid course hours-and-points line");
        }

        course.setPoints(Float.valueOf(m.group(2)));

        CourseParserState state = CourseParserState.START;

        int current_lecture_group_number = 1;

        Group group = null;

        while (current_line != null) {
            current_line = reverse(REPY_file.readLine());
            switch (state) {
            case START:
                if (current_line.charAt(3) != '-') {
                    if ((m = Expressions.LECTURER_IN_CHARGE
                            .matcher(current_line)).matches()) {
                        course.setLecturerInCharge(m.group(1).trim()
                                .replaceAll("\\s+", " "));
                    } else if ((m = Expressions.FIRST_TEST_DATE
                            .matcher(current_line)).matches()) {
                        course.setFirstTestDate(m.group(1));
                    } else if ((m = Expressions.SECOND_TEST_DATE
                            .matcher(current_line)).matches()) {
                        course.setSecondTestDate(m.group(1));
                    } else if (current_line
                            .equals(Expressions.REGISTRATION_BLANK)
                            || current_line.equals(Expressions.BLANK)) {
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
                    } else {
                        group_number = 10 * current_lecture_group_number;
                        current_lecture_group_number += 1;
                    }

                    group = new Group(group_number,
                            parse_group_type(m.group(2)));

                    Event e = parse_event_line(m.group(3));

                    if (e == null) {
                        throw parseError("Invalid event line", m.group(3));
                    }

                    group.getEvents().add(e);

                    state = CourseParserState.DETAILS;
                }
            case DETAILS:
                if (!current_line.contains(":")) {
                    if (current_line.equals(Expressions.BLANK)
                            || current_line.contains("++++++")
                            || current_line.contains("----")) {
                        if ((group != null) && (!group.getEvents().isEmpty())) {
                            course.getGroups().add(group);
                        }
                        state = CourseParserState.THING;
                    } else {
                        m = Expressions.ANYTHING.matcher(current_line);
                        if (m.matches()) {
                            group.getEvents().add(parse_event_line(m.group(1)));
                        }
                    }
                } else if ((m = Expressions.GROUP_LECTURER
                        .matcher(current_line)).matches()) {
                    group.setLecturer(m.group(1).trim());
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
            throw new ParseException(String.format(
                    "Could not figure out the following line: %s", event_line),
                    REPY_file.getLineNumber());
        }

        if (m.group(1).length() != 1) {
            throw new ParseException(String.format("Invalid day name '%s'", m
                    .group(1)), REPY_file.getLineNumber());
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
        String[] bits;
        StringBuilder sb = new StringBuilder();
        bits = s.split("\\s+");
        sb.append(bits[0]);
        sb.append(reverse(bits[1]));
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
        String name_line = reverse(REPY_file.readLine());
        String faculty_name;
        Matcher m = Expressions.FACULTY_NAME.matcher(name_line);

        if (m.matches()) {
            faculty_name = m.group(1).trim();
        } else {
            throw parseError("Invalid faculty name line");
        }

        // Read unused line describing the current semester
        REPY_file.readLine();

        if (!REPY_file.readLine().equals(Expressions.FACULTY_SEPARATOR)) {
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
