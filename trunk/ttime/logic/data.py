#!/usr/bin/env python
# -*- coding: utf-8 -*-
#
import urllib
import zipfile
import tempfile
import codecs

def repy_data():
    """Download raw REPY data from Technion, convert it to almost-unicode
    (numbers are reversed)"""
    REPY_URI = "http://ug.technion.ac.il/rep/REPFILE.zip"
    t = tempfile.TemporaryFile()
    t.write(urllib.urlopen(REPY_URI).read())
    z = zipfile.ZipFile(t)
    repy_data = '\n'.join([ unicode(x).rstrip('\r')[::-1] for x in
        unicode(z.read('REPY'), 'cp862').split('\n') ])
    z.close
    t.close
    return repy_data

# FIXME - Remove this:
if __name__ == '__main__':
    f = codecs.open('repy_data_example','w',encoding='utf-8')
    d = repy_data()
    f.write(d)
    f.close()
