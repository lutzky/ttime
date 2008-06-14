#!/usr/bin/env python

# TODO: Some code to load and save the prefs.
# Intended usage - import and use for static program-wide preferences.
# Hopefully for plugins as well

import os

SQLITE_DB_FILE = 'ttime.sqlite'

def homedir(): return os.path.expanduser('~')
def ttime_dir(): return os.path.join(homedir(),'.ttime')
def pref_file(filename): return os.path.join(ttime_dir(), filename)

def ensure_prefs_dir():
    if not os.path.isdir(ttime_dir()):
        os.mkdir(ttime_dir())

def sqlite_db_filename(): return os.path.abspath(pref_file(SQLITE_DB_FILE))

ensure_prefs_dir()
