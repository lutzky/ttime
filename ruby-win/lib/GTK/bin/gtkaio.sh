if test -n "$GTK_BASEPATH"; then
  case `uname -a` in
	CYGWIN*)
	  GTK_BASEPATH=`cygpath $GTK_BASEPATH`;;
	MINGW32*)
	  GTK_BASEPATH=`echo $GTK_BASEPATH | sed -e 's/\\\\/\\//g' -e 's/^\\([a-zA-Z]\\):/\\/\\1/g'`
  esac
  export GTK_BASEPATH=$GTK_BASEPATH
  export PATH=$GTK_BASEPATH/bin:$PATH
  export ACLOCAL_FLAGS="-I $GTK_BASEPATH/share/aclocal $ACLOCAL_FLAGS"

  if test "x$C_INCLUDE_PATH" = x; then
	APPEND=
  else
	APPEND=":$C_INCLUDE_PATH"
  fi
  export C_INCLUDE_PATH=$GTK_BASEPATH/include$APPEND

  if test "x$LIBRARY_PATH" = x; then
	APPEND=
  else
	APPEND=":$LIBRARY_PATH"
  fi
  export LIBRARY_PATH=$GTK_BASEPATH/lib:/lib/w32api$APPEND

  if test "x$PKG_CONFIG_PATH" = x; then
	APPEND=
  else
	APPEND=":$PKG_CONFIG_PATH"
  fi
  export PKG_CONFIG_PATH=$GTK_BASEPATH/lib/pkgconfig$APPEND
fi
