#!/usr/bin/env python
# -*- coding: utf-8 -*-

import codecs
import re # the awesome power of text parsing!

from ttime.logic import data

FACULTY_BANNER_REGEX = re.compile("""\+==========================================\+
\| מערכת שעות - (.*) +\|
\|.*\|
\+==========================================\+""", re.UNICODE)

COURSE_BANNER_REGEX = re.compile("""\+------------------------------------------\+
\| (\d\d\d\d\d\d) +(.*) +\|
\| שעות הוראה בשבוע:ה-(\d) ת-(\d) +נק: (.*) *\|
\+------------------------------------------\+""", re.UNICODE)

# FIXME: Finish this
def parse_repy_data():
    raw_faculties = data.repy_data().split("\n\n")
# FIXME FIXME regexes misbehaving bahhhhhhh :/
#    data_split = re.split(FACULTY_BANNER_REGEX, raw_faculties[0])
#    data_split = re.split("==", raw_faculties[0])
#    print len(data_split)
#    print data_split[41]

    return raw_faculties
