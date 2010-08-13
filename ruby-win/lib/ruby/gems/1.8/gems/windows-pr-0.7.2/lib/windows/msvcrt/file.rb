require 'windows/api'
include Windows

module Windows
   module MSVCRT
      module File
         API.auto_method   = false
         API.auto_constant = false
         API.auto_unicode  = false

         S_IFMT   = 0170000 # file type mask
         S_IFDIR  = 0040000 # directory
         S_IFCHR  = 0020000 # character special
         S_IFIFO  = 0010000 # pipe
         S_IFREG  = 0100000 # regular
         S_IREAD  = 0000400 # read permission, owner
         S_IWRITE = 0000200 # write permission, owner
         S_IEXEC  = 0000100 # execute/search permission, owner

         Chmod   = API.new('_chmod', 'PI', 'I', 'msvcrt')
         Chsize  = API.new('_chsize', 'IL', 'I', 'msvcrt')
         Mktemp  = API.new('_mktemp', 'P', 'P', 'msvcrt')
         Stat    = API.new('_stat', 'PP', 'I', 'msvcrt')
         Stat64  = API.new('_stat64', 'PP', 'I', 'msvcrt')
         Wchmod  = API.new('_wchmod', 'PI', 'I', 'msvcrt')
         Wmktemp = API.new('_wmktemp', 'P', 'P', 'msvcrt')
         Wstat   = API.new('_wstat', 'PP', 'I', 'msvcrt')
         Wstat64 = API.new('_wstat64', 'PP', 'I', 'msvcrt')

         # VC++ 8.0 or later
         begin
            Chsize_s  = API.new('_chsize_s', 'IL', 'I', 'msvcrt')
            Mktemp_s  = API.new('_mktemp_s', 'PL', 'L', 'msvcrt')
            Wmktemp_s = API.new('_wmktemp_s', 'PL', 'L', 'msvcrt')
         rescue RuntimeError
            # Ignore - you must check for it via 'defined?'
         end
         
         def chmod(file, mode)
            Chmod.call(file, mode)
         end
         
         def chsize(fd, size)
            Chsize.call(fd, size)
         end
         
         def chsize_s(fd, size)
            Chsize_s.call(fd, size)
         end

         def mktemp(template)
            Mktemp.call(template)
         end

         def mktemp_s(template, size)
            Mktemp_s.call(template, size)
         end
         
         def stat(path, buffer)
            Stat.call(path, buffer)
         end
      
         def stat64(path, buffer)
            Stat64.call(path, buffer)
         end
         
         def wchmod(file, mode)
            Wchmod.call(file, mode)
         end

         def wmktemp(template)
            Wmktemp.call(template)
         end

         def wmktemp_s(template, size)
            Wmktemp_s.call(template, size)
         end

         def wstat(path, buffer)
            Wstat.call(path, buffer)
         end
      
         def wstat64(path, buffer)
            Wstat64.call(path, buffer)
         end
      end
   end
end
