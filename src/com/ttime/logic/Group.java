package com.ttime.logic;

import java.util.HashSet;
import java.util.Set;

public class Group {
	public enum Type {
		LECTURE, TUTORIAL, LAB, OTHER, SPORTS
	}

	int number;
	Type type;

	public Type getType() {
		return type;
	}

	Set<Event> events;
	String lecturer;

	/**
	 * Title of sports group
	 *
	 * This is only really relevant if this.type = Type.SPORTS.
	 */
	String title;

	public String getLecturer() {
		return lecturer;
	}

	public void setLecturer(String lecturer) {
		this.lecturer = lecturer;
	}

	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}

	public Group(int number, Type type) {
		this.number = number;
		this.type = type;
		this.events = new HashSet<Event>();
		this.lecturer = null;
		this.title = null;
	}

	public Set<Event> getEvents() {
		return this.events;
	}

	@Override
	public String toString() {
		return String.format(
				"<Group number=%d type=%s lecturer=%s title=%s>",
				number, type, lecturer, title);
	}
}
