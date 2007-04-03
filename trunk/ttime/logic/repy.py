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

# FIXME: Finish this
def parse_repy_data():
    if prefs.options.do_parsing:
        # Functional or just plain messed up? You be the judge.
        parsed_faculties = [
                parse_raw_faculty(*tuple(
                    re.split(FACULTY_BANNER_REGEX, raw_faculty)[1:3]
                    ))
                for raw_faculty in data.repy_data().split("\n\n")
                ]

        # FIXME: Later non-standard faculties mess this up

        print parsed_faculties

        return parsed_faculties

def parse_raw_faculty(name, raw_data):
    print name
    return name

