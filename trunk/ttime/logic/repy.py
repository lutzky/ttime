#!/usr/bin/env python
# -*- coding: utf-8 -*-


from ttime.gui import warning, error
from ttime.localize import _

import codecs
import re # the awesome power of text parsing!

from ttime import prefs
from ttime.logic import data

FACULTY_BANNER_REGEX = re.compile(u"""\+==========================================\+
\| מערכת שעות - (.*) +\|
\|.*\|
\+==========================================\+""", re.UNICODE)

COURSE_BANNER_REGEX = re.compile(u"""\+------------------------------------------\+
\| (\d\d\d\d\d\d) +(.*) +\|
\| שעות הוראה בשבוע:ה-(\d) ת-(\d) +נק: (.*) *\|
\+------------------------------------------\+""", re.UNICODE)

SPORTS_BANNER_REGEX = re.compile(u"""\+===============================================================\+
| +מקצועות ספורט -.*\|
\+===============================================================\+""", re.UNICODE)

RAW_COURSE_REGEX = re.compile(u"\n("
        u"(\| מורה  אחראי :(?P<lecturer_in_charge>.*?) *\|)|"
        u"(\| מועד ראשון :(?P<first_test_date>.*?) *\|)|"
        u"(\| מועד שני   :(?P<second_test_date>.*?) *\|)|"
        u"(\| -+ +\|)"
        u"\n)*"
#        u"(\n)?((\|מס. + \++ +\|\n\|רישום +\|)|(\| +\|))"
        
        , re.UNICODE)

COURSE_HEADER_REGEX = re.compile(u"""\n\|\s(?P<course_id>\d\d\d\d\d\d) (?P<course_name>[א-תףץךןם0-9()\/+#,.\-"' ]+?) *\|\n\| שעות הוראה בשבוע\:( *[א-ת].+?[0-9]+)+ +נק: (?P<academic_points>\d\.?\d) ?\|""", re.UNICODE)

GROUP_LINE_REGEX = re.compile(u"\| *([0-9]*) *([א-ת]+) ?: ?(.*?) *\|", re.UNICODE)

STATE_START, STATE_THING, STATE_DETAILS = range(3)

# This is included due to Python not allowing assignments in if statements.
# Thus, this ruby code
#
#   if m=/some_regex/.match(line)
#     something = m[1]
#   elsif m=/some_other_regex/.match(line)
#     something_else = m[1]
#
# ...becomes this python code:
#
#   mre = Matcher() # once
#
#   if mre.match(some_regex, line):
#       something = mre.groups[0]
#   elif mre.match(other_regex, line):
#       something_else = mre.groups[0]
class Matcher:
    def match(self,regex,str):
        m = re.match(regex,str)
        if m: self.groups = m.groups()
        return m

def single_space(str): return re.sub(' +',' ', str)

def parse_repy_data():
    if prefs.options.do_parsing:
        non_sport_raw, sport_header, sport_raw = tuple(
                re.split(SPORTS_BANNER_REGEX, data.repy_data())
                )

        faculties = parse_non_sport_data(non_sport_raw)

        # FIXME: We should do something about sport_raw too, shouldn't we? :)

        return None

def parse_non_sport_data(raw_data):
    # Functional or just plain messed up? You be the judge.
    parsed_faculties = [
        parse_raw_faculty(
            *tuple(re.split(FACULTY_BANNER_REGEX, raw_faculty)[1:3])
            )
        for raw_faculty in raw_data.split("\n\n")
    ]
    return parsed_faculties


def parse_raw_faculty(name = None, raw_data = None):
    if not name:
        return None

    raw_courses = raw_data.split("+------------------------------------------+")

    courses = []

    raw_courses.pop(0) # Remove initial blank

    for i in range(len(raw_courses)/2):
        courses += parse_raw_course(header = raw_courses[i*2],
                body = raw_courses[i*2+1])

    return (name, courses) # TODO: Stick with this format?

def parse_raw_course(header, body):
    m = re.match(COURSE_HEADER_REGEX, header)
    course_id = m.groupdict()['course_id']
    course_name = m.groupdict()['course_name'].strip()
    academic_points = float(m.groupdict()['academic_points'])
    hours = []

    arr = m.groups()
    for i in range(2,len(arr)-1):
        hours.append(arr[i].strip())

    # print course_id, course_name, academic_points, str(hours), '!'

    # TODO: The state-machine :/

    current_lecture_group_number = 1

    print body

    m = re.match(RAW_COURSE_REGEX, body)

    try:
        print m.groupdict()
        for i,j in m.groupdict().items(): print i,j
    except:
        error(_(u"Error parsing course body in %s %s. Body was:\n‏<tt>%s</tt>") %
            (course_id, course_name, body))
        raise

    error("stopping")
    raise

    return (header, body)
