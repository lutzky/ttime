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
import com.ttime.logic.Group.Type;

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
                .compile("\\| *מורה +אחראי :(.*?) *\\|");

        final static Pattern GROUP_LECTURER = Pattern
                .compile("\\| *מרצה *:(.*?) *\\|");

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

    static class SportsExpressions {
        final static String FACULTY_SEPARATOR = "+===============================================================+";

        final static String COURSE_SEPARATOR = "+---------------------------------------------------------------+";

        final static String GROUP_SEPARATOR = "|            -----                                              |";

        final static String BLANK = "|                                                               |";

        final static String FIRST_COMMENT_LINE = "|       1.";

        final static Pattern SPORTS_HEADER = Pattern
                .compile("^\\| *מקצועות ספורט .*\\|$");

        static final String FACULTY_NAME = "מקצועות ספורט";

        final static Pattern SPORTS_EVENT_DETAILS = Pattern
                .compile("\\| {8}.{20}([אבגדהוש])' *(\\d{2}\\.\\d{1,2}) *- *(\\d{2}\\.\\d{1,2}) *(.*?) +\\|");

        public static final int GROUP_NUMBER_START_INDEX = 9;

        public static final int GROUP_NUMBER_END_INDEX = 11;

        public static final int GROUP_TITLE_START_INDEX = 12;

        public static final int GROUP_TITLE_END_INDEX = 29;

        final static Pattern GROUP_INSTRUCTOR = Pattern
                .compile("\\| *מדריך *:(.*?) *\\|");
    }

    enum CourseParserState {
        START, THING, DETAILS, COMMENTS
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

    Set<Faculty> faculties;

    public Set<Faculty> getFaculties() {
        return faculties;
    }

    Logger log;

    public Repy(File filename) throws IOException, ParseException {
        log = Logger.getLogger("global");

        REPY_file = new LineNumberReader(new InputStreamReader(
                new FileInputStream(filename), Charset.forName("cp862")));
        System.out.printf("Constructing a Repy from %s\n", filename);

        faculties = new HashSet<Faculty>();

        while (readRepyLine().isEmpty()) {
            // Skip leading blank lines
        }

        while (current_line.equals(Expressions.FACULTY_SEPARATOR)) {
            log.finer("Read faculty separator, parsing a faculty.");
            faculties.add(parseFaculty());
            readRepyLine();
        }

        log.fine("Done reading faculties, skipping blank lines...");

        while (readRepyLine().isEmpty()) {
            // Skip leading blank lines
        }

        faculties.add(parseSportsFaculty());
    }

    Faculty parseSportsFaculty() throws IOException, ParseException {
        log.fine("Parsing the sports section.");

        expectCurrent(SportsExpressions.FACULTY_SEPARATOR);

        if (!SportsExpressions.SPORTS_HEADER.matcher(readRepyLine()).matches()) {
            throw parseError("Invalid sports header");
        }

        expect(SportsExpressions.FACULTY_SEPARATOR);

        Faculty f = new Faculty(SportsExpressions.FACULTY_NAME);

        expect(SportsExpressions.COURSE_SEPARATOR);

        log.fine("Done reading sports faculty header");

        Course course;

        while ((current_line != null)
                && ((course = parseSportsCourse()) != null)) {
            f.getCourses().add(course);
        }

        return f;
    }

    Course parseSportsCourse() throws IOException, ParseException {
        Matcher m;
        Course course;

        CourseParserState state = CourseParserState.START;

        log.fine("Starting to parse a sports course.");

        readRepyLine();

        if (current_line == null) {
            log.fine("Null line means no more sports courses.");
            return null;
        }

        if (current_line.isEmpty()) {
            log.fine("Got an empty line - returning a null course (SPORTS)");
            return null;
        }

        if (!(m = Expressions.COURSE_NAME_NUMBER.matcher(current_line))
                .matches()) {
            throw parseError("Invalid course number-and-name line (SPORTS)");
        }

        course = new Course(Integer.valueOf(reverse(m.group(1))), m.group(2));

        log.fine(String.format("Parsing sports course %s", course));

        if (!(m = Expressions.COURSE_HOURS_POINTS.matcher(readRepyLine()))
                .matches()) {
            throw parseError("Invalid course hours-and-points line (SPORTS)");
        }

        course.setPoints(Float.valueOf(reverse(m.group(2))));

        expect(SportsExpressions.COURSE_SEPARATOR);

        log.fine(String.format("%s is a %.1f-point course", course, course
                .getPoints()));

        Group group = null;

        while (!readRepyLine().equals(SportsExpressions.COURSE_SEPARATOR)) {
            if (current_line.equals(SportsExpressions.GROUP_SEPARATOR)) {
                log.finer("Skipping meaningless dash line");
                continue;
            }

            log.finer(String.format("State is %s", state));

            switch (state) {
            case START:
                if ((m = Expressions.LECTURER_IN_CHARGE.matcher(current_line))
                        .matches()) {
                    course.setLecturerInCharge(squeeze(m.group(1)));
                    log.fine(String.format("Lecturer in charge is %s", course
                            .getLecturerInCharge()));
                } else if (current_line
                        .startsWith(SportsExpressions.FIRST_COMMENT_LINE)) {
                    log.finer("Skipping comments...");
                    state = CourseParserState.COMMENTS;
                } else if (current_line.equals(SportsExpressions.BLANK)) {
                    state = CourseParserState.THING;
                }
                break;
            case COMMENTS:
                // TODO make use of the comments?
                if (current_line.equals(SportsExpressions.BLANK)) {
                    state = CourseParserState.THING;
                }
                break;
            case THING:
                if (current_line.equals(SportsExpressions.BLANK)) {
                    continue;
                }

                Integer groupNumber = Integer.valueOf(current_line.substring(
                        SportsExpressions.GROUP_NUMBER_START_INDEX,
                        SportsExpressions.GROUP_NUMBER_END_INDEX));

                String groupTitle = current_line.substring(
                        SportsExpressions.GROUP_TITLE_START_INDEX,
                        SportsExpressions.GROUP_TITLE_END_INDEX).trim();

                group = new Group(groupNumber, Type.SPORTS);

                course.getGroups().add(group);

                group.setTitle(groupTitle);

                log.fine(String.format("Parsing group %d - \"%s\"",
                        groupNumber, groupTitle));

                try {
                    group.getEvents().add(parseSportsEventLine(course));
                } catch (ParseException e) {
                    log
                            .warning(String
                                    .format(
                                            "Ignoring group %s (REPY line %d), as it has no valid details",
                                            group, REPY_file.getLineNumber()));
                    group = null;
                }

                state = CourseParserState.DETAILS;
                break;
            case DETAILS:
                if (current_line.equals(SportsExpressions.BLANK)) {
                    state = CourseParserState.THING;
                    continue;
                }

                if (group == null) {
                    throw parseError("Got group details before a start-of-group line (SPORTS)");
                }

                if ((m = SportsExpressions.GROUP_INSTRUCTOR
                        .matcher(current_line)).matches()) {
                    group.setLecturer(squeeze(m.group(1).trim()));
                    log.fine(String.format("Group lecturer for %s is %s",
                            group, group.getLecturer()));
                } else {
                    group.getEvents().add(parseSportsEventLine(course));
                }
                break;
            }
        }

        return course;
    }

    Event parseSportsEventLine(Course course) throws ParseException {
        // TODO this has a lot of shared code with parsing of normal event
        // lines, do some merging

        Matcher m = SportsExpressions.SPORTS_EVENT_DETAILS
                .matcher(current_line);

        if (!m.matches()) {
            throw parseError("Invalid sports group event details");
        }

        Event e = new Event(course, dayLetterToNumber(m.group(1).charAt(0)),
                parseTime(m.group(2)), parseTime(m.group(3)), m.group(4).trim());

        log.fine(String.format("Got event %s", e));

        return e;
    }

    void expectCurrent(String s) throws IOException, ParseException {
        if (!current_line.equals(s)) {
            throw parseError(String.format("Expected \"%s\"", s));
        }
    }

    void expect(String s) throws IOException, ParseException {
        readRepyLine();
        expectCurrent(s);
    }

    String readRepyLine() throws IOException {
        String backwards_line = REPY_file.readLine();
        if (backwards_line == null) {
            log.finer("End of REPY file");
            current_line = null;
            return null;
        }
        current_line = reverse(backwards_line);
        log.finest(String.format("REPY: %s", current_line));
        return current_line;
    }

    Faculty parseFaculty() throws IOException, ParseException {
        Course current_course;

        Faculty faculty = new Faculty(parseFacultyHeader());

        log.fine(String.format(
                "Got a faculty, %s, and finished parsing its header.",
                faculty.getName()));

        while ((current_course = parseCourse()) != null) {
            log.fine(String.format("Done parsing course <%d - %s>, adding it",
                    current_course.getNumber(), current_course.getName()));
            faculty.getCourses().add(current_course);
        }

        return faculty;
    }

    Course parseCourse() throws NoSuchElementException, IOException,
            ParseException {
        // TODO parseSportsCourse is done better, consider using a similar style
        readRepyLine();

        if (current_line.isEmpty()) {
            return null;
        }

        if (current_line.equals(Expressions.COURSE_SEPARATOR)) {
            log.fine("Throwing away extraneous course separator line.");
            readRepyLine();
        }

        Matcher m = Expressions.COURSE_NAME_NUMBER.matcher(current_line);

        if (!m.matches()) {
            throw parseError("Invalid course name-and-number line",
                    current_line);
        }

        int course_number = Integer.valueOf(reverse(m.group(1)));
        String course_name = m.group(2).trim();

        Course course = new Course(course_number, course_name);

        log.fine(String.format("Got course name and number %d - %s",
                course_number, course_name));

        readRepyLine();

        m = Expressions.COURSE_HOURS_POINTS.matcher(current_line);

        if (!m.matches()) {
            throw parseError("Invalid course hours-and-points line");
        }

        course.setPoints(Float.valueOf(reverse(m.group(2))));

        log.fine(String.format("This is a %.1f-point course", course
                .getPoints()));

        expect(Expressions.COURSE_SEPARATOR);

        log.finer("Got end of course header");

        log.finer(String.format("Starting to parse course %d - %s",
                course_number, course_name));

        log.finer("Setting state to START");
        CourseParserState state = CourseParserState.START;

        log.finer("Setting current lecture group number to 1");
        int current_lecture_group_number = 1;

        Group group = null;

        while (!readRepyLine().equals(Expressions.COURSE_SEPARATOR)) {
            log.finer(String.format("State is %s", state));
            switch (state) {
            case START:
                if (current_line.charAt(3) != '-') {
                    if ((m = Expressions.LECTURER_IN_CHARGE
                            .matcher(current_line)).matches()) {
                        log.fine("Got valid lecturer in charge");
                        course.setLecturerInCharge(squeeze(m.group(1)));
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
                        log.finer("Changing state to THING");
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
                        log.fine(String.format("Got group number %d",
                                group_number));
                    } else {
                        group_number = 10 * current_lecture_group_number;
                        current_lecture_group_number += 1;
                        log.fine(String.format(
                                "Got no group number, guessing by count to %d",
                                group_number));
                    }

                    group = new Group(group_number,
                            parseGroupType(m.group(2)));

                    Event e = parseEventLine(course, m.group(3));

                    if (e == null) {
                        log.fine("Blank event line, ignoring.");
                    }

                    log.fine("Got a valid event line.");

                    group.getEvents().add(e);

                    log.finer("Setting state to DETAILS.");

                    state = CourseParserState.DETAILS;
                }
                break;
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
                        } else {
                            log.fine("No or empty group to add, not adding");
                        }

                        log.finer("Setting state to THING.");
                        state = CourseParserState.THING;
                    } else {
                        m = Expressions.ANYTHING.matcher(current_line);
                        if (m.matches()) {
                            group.getEvents().add(
                                    parseEventLine(course, m.group(1)));
                            log.fine("Added a valid event line.");
                        }
                    }
                } else if ((m = Expressions.GROUP_LECTURER
                        .matcher(current_line)).matches()) {
                    group.setLecturer(squeeze(m.group(1).trim()));
                    log.fine("Got a valid group lecturer.");
                }
                break;
            }
        }

        if (state == CourseParserState.DETAILS) {
            if (!group.getEvents().isEmpty()) {
                course.getGroups().add(group);
            }
        }

        return course;
    }

    String squeeze(String s) {
        return s.trim().replaceAll(" +", " ");
    }

    /**
     * @param s
     *            Time string in the form HH.MM, 24-hour, HH may be single-digit
     *            (MM may not)
     * @return Seconds from midnight to s
     * @throws ParseException
     */
    int parseTime(String s) throws ParseException {
        try {
            String bits[] = reverse(s).split("\\.");
            if (bits.length != 2) {
                throw new ParseException("", 0);
            }
            int seconds = Integer.valueOf(bits[0]) * 3600
                    + Integer.valueOf(bits[1]) * 60;
            return seconds;
        } catch (Exception e) {
            throw parseError("Could not parse time", s);
        }
    }

    Event parseEventLine(Course course, String event_line)
            throws ParseException {
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

        return new Event(course, dayLetterToNumber(day_letter),
                parseTime(m.group(3)), parseTime(m.group(2)), place_fix(m
                        .group(4)));
    }

    int dayLetterToNumber(Character day_letter) {
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
        if (s.trim().isEmpty()) {
            return null;
        }
        String[] bits;
        StringBuilder sb = new StringBuilder();
        bits = s.split(" +");
        sb.append(bits[0]);
        sb.append(" ");
        if (bits.length > 1) {
            try {
                if (String.valueOf(Integer.valueOf(bits[1])).equals(bits[1])) {
                    // bits[1] is numeric, we need to reverse it
                    sb.append(reverse(bits[1]));
                } else {
                    sb.append(bits[1]);
                }
            } catch (NumberFormatException e) {
                log.finest(String.format(
                        "place_fix(aaa \"%s\" aaa) == aaa \"%s\" aaa", s, s));
                return s;
            }
        }
        log.finest(String.format("place_fix(aaa \"%s\" aaa) == aaa \"%s\" aaa",
                s, sb.toString()));
        return sb.toString();
    }

    Group.Type parseGroupType(String s) {
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

    String parseFacultyHeader() throws IOException, ParseException {
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

        expect(Expressions.FACULTY_SEPARATOR);

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
