from sqlobject import *
from datetime import date

import os
import sys

from ttime import prefs

class Lecturer(SQLObject):
    name = StringCol()
    
class Semester(SQLObject):
    year = IntCol()
    semester = EnumCol(enumValues = ["Winter", "Spring", "Summer"])

class Course(SQLObject):
    number = StringCol(length = 6)
    semester = ForeignKey("Semester")
    name = StringCol()
    
    academic_points = DecimalCol(size = 1, precision = 1)
    academic_hours = IntCol()
    first_exam_date = DateCol()
    second_exam_date = DateCol()
    lecturer_in_charge = ForeignKey("Lecturer")
    
    event_sets = MultipleJoin("EventSet")

class EventSet(SQLObject):
    course = ForeignKey("Course")
    type = EnumCol(enumValues = ["Lecture", "Tutorial", "Laboratory", "Set"])
    
    event = MultipleJoin("Event")

days_of_the_week = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]

class Event(SQLObject):
    lecturer = ForeignKey("Lecturer")
    set = ForeignKey("EventSet")
    place = ForeignKey("Place")
    day_of_week = EnumCol(enumValues = days_of_the_week)
    start_time = DecimalCol(size = 2, precision = 2)
    end_time = DecimalCol(size = 2, precision = 2)


class Place(SQLObject):
    name = StringCol()

_all_tables = [EventSet, Event, Place, Course, Semester, Lecturer]

def test():
    test_create_db()
    test_add_course()
    test_print_all_courses()

def test_uri():
    db_filename = "testfile.sqlite"
    uri = "sqlite:%s" % os.path.abspath(db_filename)
    #uri = "sqlite:/:memory:"
    return uri

def test_create_db():
    connection = connectionForURI("sqlite:%s" % prefs.sqlite_db_filename())
    sqlhub.processConnection = connection
    
    for table in _all_tables:
        table.dropTable(ifExists = True)
        table.createTable()

def test_add_course():
    t = Place(name = "Taub")
    l = Lecturer(name = "Kimchi")
    s = Semester(year = 1992, semester = "Winter")
    c = Course(number = "042344", semester = s, name = 
        "Test Course", academic_points = 5.5, academic_hours = 1,
        first_exam_date = date(1992,1,12), 
        second_exam_date = date(1992,12,1),
        lecturer_in_charge = l)

def test_print_all_courses():
    for c in Course.select():
        print "%s - %s, by %s\n" % (c.number, c.name, c.lecturer_in_charge.name)
        print "Exam: %s" % c.first_exam_date
