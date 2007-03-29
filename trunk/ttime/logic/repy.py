#!/usr/bin/env python
# -*- coding: utf-8 -*-

import codecs
import re # the awesome power of text parsing!

FACULTY_BANNER_REGEX = re.compile("""\+==========================================\+
\| מערכת שעות - (.*) +\|
\|.*\|
\+==========================================\+""", re.UNICODE)

COURSE_BANNER_REGEX = re.compile("""\+------------------------------------------\+
\| (\d\d\d\d\d\d) +(.*) +\|
\| שעות הוראה בשבוע:ה-(\d) ת-(\d) +נק: (.*) *\|
\+------------------------------------------\+""", re.UNICODE)

# FIXME: Finish this
def parse_repy_data(raw_repy_data):
    raw_faculties = raw_repy_data.split("\n\n")

    return raw_faculties

# FIXME: Remove this
if __name__ == "__main__":
    import data
    print len(parse_repy_data(data.repy_data()))
