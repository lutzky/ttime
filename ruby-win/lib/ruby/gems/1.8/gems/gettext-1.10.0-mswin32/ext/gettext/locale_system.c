/* -*- c-file-style: "ruby"; tab-width: 4 -*-
 *
 * Copyright (C) 2002-2006 Masao Mutoh
 * 
 * You may redistribute it and/or modify it under the same
 * license terms as Ruby.
 * 
 * $Id: locale_system.c,v 1.6 2006/06/04 14:43:37 mutoh Exp $
 */

#include "ruby.h"

#if defined HAVE_WINDOWS_H
# include <windows.h>
#else
# if defined HAVE_SETLOCALE
#  include <locale.h>
# endif
# if defined HAVE_NL_LANGINFO
#  include <langinfo.h>
# endif
#endif
#ifndef StringValuePtr
#define StringValuePtr(s) STR2CSTR(s)
#endif

#if defined HAVE_WINDOWS_H
static VALUE
gt_locale_id_win32(self)
    VALUE self;
{
    return INT2NUM(GetUserDefaultLangID());
}
#endif
static VALUE
gt_setlocale(self, type, locale)
    VALUE self, type, locale;
{
# if defined HAVE_SETLOCALE
    char* ret = setlocale(NUM2INT(type), 
			  NIL_P(locale) ? NULL : StringValuePtr(locale));
    return ret == NULL ? Qnil : rb_str_new2(ret);
# else
    return Qnil;
# endif
}

static VALUE
gt_codeset(self)
	VALUE self;
{
#if defined HAVE_WINDOWS_H
	/* This isn't used now. See lib/gettext/locale_win32.rb */
	char buf[2 + 10 + 1];
	sprintf (buf, "CP%u", GetACP ());
	return rb_str_new2(buf);
#elif defined HAVE_NL_LANGINFO && defined CODESET
	return rb_str_new2(nl_langinfo(CODESET));
#else
	return Qnil;
#endif
}

void Init_locale_system()
{
    VALUE mLocale = rb_define_module("Locale");
    VALUE mSystem = rb_define_module_under(mLocale, "System");
#if defined HAVE_WINDOWS_H
    rb_define_module_function(mSystem, "locale_id", gt_locale_id_win32, 0);
#endif
    rb_define_module_function(mSystem, "set", gt_setlocale, 2);
    rb_define_module_function(mSystem, "codeset", gt_codeset, 0);
# if defined HAVE_SETLOCALE
    rb_define_const(mSystem, "ALL", INT2NUM(LC_ALL));
    rb_define_const(mSystem, "COLLATE", INT2NUM(LC_COLLATE));
    rb_define_const(mSystem, "CTYPE", INT2NUM(LC_CTYPE));
    rb_define_const(mSystem, "MESSAGES", INT2NUM(LC_MESSAGES));
    rb_define_const(mSystem, "MONETARY", INT2NUM(LC_MONETARY));
    rb_define_const(mSystem, "NUMERIC", INT2NUM(LC_NUMERIC));
    rb_define_const(mSystem, "TIME", INT2NUM(LC_TIME));
#endif

}
