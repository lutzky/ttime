require 'windows/api'
include Windows

module Windows
   module MSVCRT
      module Directory
         API.auto_method   = false
         API.auto_constant = false
         API.auto_unicode  = false

         Chdir       = API.new('_chdir', 'P', 'I', 'msvcrt')
         Wchdir      = API.new('_wchdir', 'P', 'I', 'msvcrt')
         Chdrive     = API.new('_chdrive', 'I', 'I', 'msvcrt')
         Getcwd      = API.new('_getcwd', 'PI', 'P', 'msvcrt')
         Wgetcwd     = API.new('_wgetcwd', 'PI', 'P', 'msvcrt')
         Getdcwd     = API.new('_getdcwd', 'IPI', 'P', 'msvcrt')
         Wgetdcwd    = API.new('_wgetdcwd', 'IPI', 'P', 'msvcrt')
         Getdiskfree = API.new('_getdiskfree', 'IP', 'I', 'msvcrt')
         Getdrive    = API.new('_getdrive', 'V', 'I', 'msvcrt')
         Getdrives   = API.new('_getdrives', 'V', 'L', 'msvcrt')
         Mkdir       = API.new('_mkdir', 'P', 'I', 'msvcrt')
         Wmkdir      = API.new('_wmkdir', 'P', 'I', 'msvcrt')
         Rmdir       = API.new('_rmdir', 'P', 'I', 'msvcrt')
         Wrmdir      = API.new('_wrmdir', 'P', 'I', 'msvcrt')
         Searchenv   = API.new('_searchenv', 'PPP', 'V', 'msvcrt')
         Wsearchenv  = API.new('_wsearchenv', 'PPP', 'V', 'msvcrt')

         def chdir(dirname)
            Chdir.call(dirname)
         end

         def wchdir(dirname)
            Wchdir.call(dirname)
         end

         def chdrive(drive_number)
            Chdrive.call(drive_number)
         end

         def getcwd(buffer, maxlen)
            Getcwd.call(buffer, maxlen)
         end

         def wgetcwd(buffer, maxlen)
            Wgetcwd.call(buffer, maxlen)
         end

         def getdcwd(drive, buffer, maxlen)
            Getdcwd.call(drive, buffer, maxlen)
         end

         def wgetdcwd(drive, buffer, maxlen)
            Wgetdcwd.call(drive, buffer, maxlen)
         end

         def getdiskfree(drive, struct_ptr)
            Getdiskfree.call(drive, struct_ptr)
         end

         def getdrive
            Getdrive.call
         end

         def getdrives
            Getdrives.call
         end

         def mkdir(dirname)
            Mkdir.call(dirname)
         end

         def wmkdir(dirname)
            Wmkdir.call(dirname)
         end

         def rmdir(dirname)
            Rmdir.call(dirname)
         end

         def wrmdir(dirname)
            Wrmdir.call(dirname)
         end

         def searchenv(filename, varname, pathname)
            Searchenv.call(filename, varname, pathname)
         end

         def wsearchenv(filename, varname, pathname)
            Wsearchenv.call(filename, varname, pathname)
         end
      end
   end
end
