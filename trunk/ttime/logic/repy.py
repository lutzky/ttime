#!/usr/bin/env python
# -*- coding: utf-8 -*-

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

COURSE_HEADER_REGEX = re.compile(u"""\n\|\s(\d\d\d\d\d\d) ([א-תףץךןם0-9()\/+#,.\-"' ]+?) *\|\n\| שעות הוראה בשבוע\:( *[א-ת].+?[0-9]+)+ +נק: (\d\.?\d) ?\|""", re.UNICODE)

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
    arr = re.match(COURSE_HEADER_REGEX, header).groups()
    course_id = arr[0]
    course_name = arr[1].strip()
    academic_points = float(arr[-1])
    hours = []

    for i in range(2,len(arr)-1):
        hours.append(arr[i].strip())

    # print course_id, course_name, academic_points, str(hours), '!'

    # TODO: The state-machine :/

    return (header, body)
