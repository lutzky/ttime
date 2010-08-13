require 'windows/api'
include Windows

# This module includes stream I/O, low level I/O, etc.
module Windows
   module MSVCRT
      module IO
         API.auto_constant = false
         API.auto_method   = false
         API.auto_unicode  = false

         S_IFMT   = 0170000 # file type mask
         S_IFDIR  = 0040000 # directory
         S_IFCHR  = 0020000 # character special
         S_IFIFO  = 0010000 # pipe
         S_IFREG  = 0100000 # regular
         S_IREAD  = 0000400 # read permission, owner
         S_IWRITE = 0000200 # write permission, owner
         S_IEXEC  = 0000100 # execute/search permission, owner

         Clearerr  = API.new('clearerr', 'I', 'V', 'msvcrt')
         Close     = API.new('_close', 'I', 'V', 'msvcrt')
         Fclose    = API.new('fclose', 'I', 'I', 'msvcrt')
         Fcloseall = API.new('_fcloseall', 'V', 'I', 'msvcrt')
         Fdopen    = API.new('_fdopen', 'IP', 'I', 'msvcrt')
         Feof      = API.new('feof', 'I', 'I', 'msvcrt')
         Ferror    = API.new('ferror', 'L', 'I', 'msvcrt')
         Fflush    = API.new('fflush', 'I', 'I', 'msvcrt')
         Fgetc     = API.new('fgetc', 'L', 'I', 'msvcrt')
         Fgetpos   = API.new('fgetpos', 'LP', 'I', 'msvcrt')
         Fgetwc    = API.new('fgetwc', 'L', 'I', 'msvcrt')
         Fgets     = API.new('fgets', 'PIL', 'P', 'msvcrt')
         Fgetws    = API.new('fgetws', 'PIL', 'P', 'msvcrt')
         Fileno    = API.new('_fileno', 'I', 'I', 'msvcrt')
         Flushall  = API.new('_flushall', 'V', 'I', 'msvcrt')
         Fopen     = API.new('fopen', 'PP', 'I', 'msvcrt')
         Fputs     = API.new('fputs', 'PL', 'I', 'msvcrt')
         Fputws    = API.new('fputws', 'PL', 'I', 'msvcrt')
         Getc      = API.new('getc', 'L', 'I', 'msvcrt')
         Getwc     = API.new('getwc', 'L', 'L', 'msvcrt')
         Open      = API.new('_open', 'PPI', 'I', 'msvcrt')
         Rmtmp     = API.new('_rmtmp', 'V', 'I', 'msvcrt')
         Tempnam   = API.new('_tempnam', 'PP', 'P', 'msvcrt')
         Tmpfile   = API.new('tmpfile', 'V', 'L', 'msvcrt')
         Tmpnam    = API.new('tmpnam', 'P', 'P', 'msvcrt')
         Wopen     = API.new('_wopen', 'PPI', 'I', 'msvcrt')
         Wfdopen   = API.new('_wfdopen', 'IP', 'I', 'msvcrt')
         Wfopen    = API.new('_wfopen', 'PPI', 'I', 'msvcrt')
         Wtempnam  = API.new('_wtempnam', 'PP', 'P', 'msvcrt')
         Wtmpnam   = API.new('_wtmpnam', 'P', 'P', 'msvcrt')

         # VC++ 8.0 or later
         begin
            Tmpfile_s = API.new('_tmpfile_s', 'P', 'L', 'msvcrt')
         rescue RuntimeError
            # Ignore - you must check for it via 'defined?'
         end

         def clearerr(stream)
            Clearerr.call(stream)
         end

         def close(fd)
            Close.call(fd)
         end

         def fclose(stream)
            Fclose.call(stream)
         end

         def fcloseall
            Fcloseall.call
         end

         def fdopen(fd, mode)
            Fdopen.call(fd, mode)
         end

         def feof(stream)
            Feof.call(stream)
         end

         def ferror(stream)
            Ferror.call(stream)
         end

         def fflush(stream)
            Fflush.call(stream)
         end

         def fgetc(stream)
            Fgetc.call(stream)
         end

         def fgetpos(stream, pos)
            Fgetpos.call(stream, pos)
         end

         def fgets(str, n, stream)
            Fgets.call(str, n, stream)
         end

         def fgetws(str, n, stream)
            Fgetws.call(str, n, stream)
         end

         def fgetwc(stream)
            Fgetwc.call(stream)
         end

         def fileno(stream)
            Fileno.call(stream)
         end

         def flushall
            Flushall.call()
         end

         def fopen(file, mode)
            Fopen.call(file, mode)
         end

         def fputs(str, stream)
            Fputs.call(str, stream)
         end

         def fputws(str, stream)
            Fputws.call(str, stream)
         end
         
         def getc(stream)
            Getc.call(stream)
         end
         
         def getwc(stream)
            Getwc.call(stream)
         end

         def open(file, flag, mode)
            Open.call(file, flag, mode)
         end

         def tmpfile()
            Tmpfile.call
         end

         def tmpfile_s(file_ptr)
            Tmpfile_s.call(file_ptr)
         end

         def tempnam(dir, prefix)
            Tempnam.call(dir, prefix)
         end

         def tmpnam(template)
            Tmpnam.call(template)
         end

         def wopen(file, flag, mode)
            Wopen.call(file, flag, mode)
         end

         def wfdopen(fd, mode)
            Wfdopen.call(fd, mode)
         end

         def wfopen(file, mode)
            Wfopen.call(file, mode)
         end

         def wtempnam(dir, prefix)
            Wtempnam.call(dir, prefix)
         end

         def wtmpnam(template)
            Wtmpnam.call(template)
         end
      end
   end
end
