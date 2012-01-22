package com.ttime.tests;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.fail;

import java.lang.reflect.Method;

import org.junit.Test;

import com.ttime.parse.Repy;

public class RepyTest {
	static class RepyStaticMethods {
		static String reverse(String s) {
			try {
				Method m = Repy.class
						.getDeclaredMethod("reverse", String.class);
				m.setAccessible(true);
				return (String) m.invoke(Repy.class, s);
			} catch (Exception e) {
				throw new RuntimeException(e);
			}
		}
	}

	@Test
	public void testReverse() {
		assertEquals("", RepyStaticMethods.reverse(""));
		assertEquals("103", RepyStaticMethods.reverse("301"));
		assertEquals("103", RepyStaticMethods.reverse("30100"));
		assertEquals("103", RepyStaticMethods.reverse("3010"));
		assertEquals("30א", RepyStaticMethods.reverse("א03"));
	}

	@Test
	public void testParseSportsFaculty() {
		fail("Not yet implemented");
	}

	@Test
	public void testParseSportsCourse() {
		fail("Not yet implemented");
	}

	@Test
	public void testParseSportsEventLine() {
		fail("Not yet implemented");
	}

	@Test
	public void testExpectCurrent() {
		fail("Not yet implemented");
	}

	@Test
	public void testExpect() {
		fail("Not yet implemented");
	}

	@Test
	public void testReadRepyLine() {
		fail("Not yet implemented");
	}

	@Test
	public void testParseFaculty() {
		fail("Not yet implemented");
	}

	@Test
	public void testParseCourse() {
		fail("Not yet implemented");
	}

	@Test
	public void testSqueeze() {
		fail("Not yet implemented");
	}

	@Test
	public void testParseTime() {
		fail("Not yet implemented");
	}

	@Test
	public void testParseEventLine() {
		fail("Not yet implemented");
	}

	@Test
	public void testDayLetterToNumber() {
		fail("Not yet implemented");
	}

	@Test
	public void testPlace_fix() {
		fail("Not yet implemented");
	}

	@Test
	public void testParseGroupType() {
		fail("Not yet implemented");
	}

	@Test
	public void testParseFacultyHeader() {
		fail("Not yet implemented");
	}

	@Test
	public void testParseErrorString() {
		fail("Not yet implemented");
	}

	@Test
	public void testParseErrorStringString() {
		fail("Not yet implemented");
	}

}
