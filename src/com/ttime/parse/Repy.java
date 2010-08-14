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
    }

    class RawCourse {

    }

    static String reverse(String s) {
        return new StringBuffer(s).reverse().toString();
    }

    class RawFaculty {
        public String name;
        public Set<RawCourse> courses;

        public RawFaculty(String name) {
            this.name = name;
        }

        public String toString() {
            return String.format("<RawFaculty \"%s\">", name);
        }
    }

    protected LineNumberReader REPY_file;
    protected String current_line;
    protected RawFaculty current_faculty;

    Set<RawFaculty> faculties;

    public Repy(File filename) throws IOException, ParseException {
        REPY_file = new LineNumberReader(new InputStreamReader(
                new FileInputStream(filename), Charset.forName("cp862")));
        System.out.printf("Constructing a Repy from %s\n", filename);

        faculties = new HashSet<RawFaculty>();

        while ((current_line = REPY_file.readLine()) != null) {
            if (current_line.equals(Expressions.FACULTY_SEPARATOR)) {
                parse_a_faculty();
            }
        }

        System.out.println("Got the following faculties:");
        for (RawFaculty rf : faculties) {
            System.out.println(rf);
        }
    }

    protected void parse_a_faculty() throws IOException, ParseException {
        RawCourse current_course;

        current_faculty = new RawFaculty(parse_faculty_header());

        try {
            while ((current_course = parse_a_course()) != null) {
                current_faculty.courses.add(current_course);
            }
        } catch (NoSuchElementException e) {
            // No more courses in this faculty. Next!
        }

        faculties.add(current_faculty);
    }

    protected RawCourse parse_a_course() throws NoSuchElementException,
            IOException, ParseException {
        String line = REPY_file.readLine();

        if (line.isEmpty()) {
            throw new NoSuchElementException();
        }
        if (!line.equals(Expressions.COURSE_SEPARATOR)) {
            throw new ParseException("Expected course header", REPY_file
                    .getLineNumber());
        }

        line = reverse(REPY_file.readLine());
        Matcher m = Expressions.COURSE_NAME_NUMBER.matcher(line);

        if (!m.matches()) {
            throw new ParseException(String.format(
                    "Invalid course name-and-number line: %s", line), REPY_file
                    .getLineNumber());
        }

        int course_number = Integer.valueOf(reverse(m.group(1)));
        String course_name = m.group(2).trim();

        line = reverse(REPY_file.readLine());
        m = Expressions.COURSE_HOURS_POINTS.matcher(line);

        if (!m.matches()) {
            throw new ParseException(String.format(
                    "Invalid course hours-and-points line: %s", line),
                    REPY_file.getLineNumber());
        }

        float points = Float.valueOf(m.group(2));

        throw new ParseException(String.format("Got course %.1f %s %06d",
                points, course_name, course_number), REPY_file.getLineNumber());

        // return new RawCourse();
    }

    protected String parse_faculty_header() throws IOException, ParseException {
        String name_line = reverse(REPY_file.readLine());
        String faculty_name;
        Matcher m = Expressions.FACULTY_NAME.matcher(name_line);

        if (m.matches()) {
            faculty_name = m.group(1).trim();
        } else {
            throw new ParseException("Invalid faculty name line", REPY_file
                    .getLineNumber());
        }

        // Read unused line describing the current semester
        REPY_file.readLine();

        if (!REPY_file.readLine().equals(Expressions.FACULTY_SEPARATOR)) {
            throw new ParseException("Expected end of faculty header",
                    REPY_file.getLineNumber());
        }

        return faculty_name;
    }
}
