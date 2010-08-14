package com.ttime.parse;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.LineNumberReader;
import java.nio.charset.Charset;
import java.text.ParseException;
import java.util.HashSet;
import java.util.Set;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Repy {
	static class Expressions {
		final static String FACULTY_SEPARATOR =
			"+==========================================+";
		
		final static Pattern FACULTY_NAME = 
			Pattern.compile("\\|\\sמערכת\\sשעות\\s-\\s(.*)\\s+\\|");      
	}
	
	class RawFaculty {
		public String name;
		
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
		String name_line = new StringBuffer(
				REPY_file.readLine()).reverse().toString();
		Matcher m = Expressions.FACULTY_NAME.matcher(name_line);
		
		if (m.matches()) {
			current_faculty = new RawFaculty(m.group(1).trim());
		}
		else {
			throw new ParseException("Invalid faculty name line",
					REPY_file.getLineNumber());
		}
		
		// Read unused line describing the current semester
		REPY_file.readLine();
		
		if (!REPY_file.readLine().equals(Expressions.FACULTY_SEPARATOR)) {
			throw new ParseException("Expected end of faculty header",
					REPY_file.getLineNumber());
		}
		
		faculties.add(current_faculty);
	}
}
