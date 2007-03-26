#!/usr/bin/env python
# -*- coding: utf-8 -*-

import codecs
import re # the awesome power of text parsing!

#####
# static constant
# TODO: do this the correct way
num_regexp = re.compile("[0-9][0-9\.-]*[0-9]")

def f(string):
    if string:
        revwords = re.split(r'([אבגדהוזחטיכלמנסעפצקרשת])+', string)    # separators too since '(...)'
        revwords.reverse()              # inplace reverse the list
        return ''.join(revwords)    # list of strings -> string
        # *NOTE* the nullstring-joiner once again!
   

# fix the hebrew
def bidi_flip(string):
    matches = num_regexp.finditer(string)
    for match in matches:
        string.replace(match,f(match))#the ::-1 notation is a crappy reverse
    return f(string)



#########################################
# actual code

repy = codecs.open('REPY','r',encoding="cp862")

for line in repy:
    print bidi_flip(line.encode('utf8','replace'))


