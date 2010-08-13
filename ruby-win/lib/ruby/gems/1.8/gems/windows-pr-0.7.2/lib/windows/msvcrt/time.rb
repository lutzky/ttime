require 'windows/api'
include Windows

module Windows
   module MSVCRT
      module Time
         API.auto_constant = false
         API.auto_method   = false
         API.auto_unicode  = false

         Asctime     = API.new('asctime', 'P', 'P', 'msvcrt')
         Clock       = API.new('clock', 'V', 'L', 'msvcrt')
         Ctime       = API.new('ctime', 'P', 'P', 'msvcrt')
         Ctime64     = API.new('_ctime64', 'P', 'P', 'msvcrt')
         Difftime    = API.new('difftime', 'LL', 'L', 'msvcrt')
         Ftime       = API.new('_ftime', 'P', 'L', 'msvcrt')
         Ftime64     = API.new('_ftime64', 'P', 'L', 'msvcrt')
         Futime      = API.new('_futime', 'IP', 'I', 'msvcrt')
         Futime64    = API.new('_futime64', 'IP', 'I', 'msvcrt')
         Gmtime      = API.new('gmtime', 'P', 'P', 'msvcrt')
         Gmtime64    = API.new('_gmtime64', 'P', 'P', 'msvcrt')
         Localtime   = API.new('localtime', 'P', 'P', 'msvcrt')
         Localtime64 = API.new('_localtime64', 'P', 'P', 'msvcrt')
         Mktime      = API.new('mktime', 'P', 'L', 'msvcrt')
         Mktime64    = API.new('_mktime64', 'P', 'L', 'msvcrt')
         Strdate     = API.new('_strdate', 'P', 'P', 'msvcrt')
         Strftime    = API.new('strftime', 'PLPP', 'L', 'msvcrt')
         Strtime     = API.new('_strtime', 'P', 'P', 'msvcrt')
         WinTime     = API.new('time', 'P', 'L', 'msvcrt')    # Avoid conflict
         WinTime64   = API.new('_time64', 'P', 'L', 'msvcrt') # Avoid conflict
         Tzset       = API.new('_tzset', 'V', 'V', 'msvcrt')
         Utime       = API.new('_utime', 'PP', 'I', 'msvcrt')
         Utime64     = API.new('_utime64', 'PP', 'I', 'msvcrt')
         Wasctime    = API.new('_wasctime', 'P', 'P', 'msvcrt')
         Wctime      = API.new('_wctime', 'P', 'P', 'msvcrt')
         Wctime64    = API.new('_wctime64', 'P', 'P', 'msvcrt')
         Wstrdate    = API.new('_wstrdate', 'P', 'L', 'msvcrt')
         Wcsftime    = API.new('wcsftime', 'PLPP', 'L', 'msvcrt')
         Wstrtime    = API.new('_wstrtime', 'P', 'P', 'msvcrt')
         Wutime      = API.new('_wutime', 'PP', 'I', 'msvcrt')
         Wutime64    = API.new('_wutime64', 'PP', 'I', 'msvcrt')

         def asctime(timeptr)
            Asctime.call(timeptr)
         end

         def clock
            Clock.call
         end

         def ctime(timer)
            Ctime.call(timer)
         end

         def ctime64(timer)
            Ctime64.call(timer)
         end

         def difftime(timer1, timer0)
            Difftime.call(timer1, timer0)
         end

         def ftime(timeptr)
            Ftime.call(timeptr)
         end

         def ftime64(timeptr)
            Ftime64.call(timeptr)
         end

         def futime(fd, filetime)
            Futime.call(fd, filetime)
         end

         def futime64(fd, filetime)
            Futime64.call(fd, filetime)
         end

         def gmtime(timer)
            Gmtime.call(timer)
         end

         def gmtime64(timer)
            Gmtime64.call(timer)
         end

         def localtime(timer)
            Localtime.call(timer)
         end

         def localtime64(timer)
            Localtime64.call(timer)
         end

         def mktime(timeptr)
            Mktime.call(timeptr)
         end

         def mktime64(timeptr)
            Mktime64.call(timeptr)
         end

         def strdate(datestr)
            Strdate.call(datestr)
         end

         def strftime(dest, maxsize, format, timeptr)
            Strftime.call(dest, maxsize, format, timeptr)
         end

         def strtime(timestr)
            Strtime.call(timestr)
         end

         def time(timer)
            WinTime.call(timer)
         end

         def time64(timer)
            WinTime64.call(timer)
         end

         def tzset
            Tzset.call
         end

         def utime(filename, times)
            Utime.call(filename, times)
         end

         def utime64(filename, times)
            Utime64.call(filename, times)
         end

         def wasctime(timeptr)
            Wasctime.call(timeptr)
         end

         def wcsftime(dest, maxsize, format, timeptr)
            Wcsftime.call(dest, maxsize, format, timeptr)
         end

         def wctime(timer)
            Wctime.call(timer)
         end

         def wctime64(timer)
            Wctime64.call(timer)
         end

         def wstrdate(datestr)
            Wstrdate.call(datestr)
         end

         def wstrtime(timestr)
            Wstrtime.call(timestr)
         end

         def wutime(filename, times)
            Wutime.call(filename, times)
         end

         def wutime64(filename, times)
            Wutime64.call(filename, times)
         end
      end
   end
end
