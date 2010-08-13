require 'windows/api'
include Windows

module Windows
   module MSVCRT
      module Buffer
         API.auto_constant = false
         API.auto_method   = false
         API.auto_unicode  = false

         Memcpy   = API.new('memcpy', 'PLL', 'P', 'msvcrt')
         Memccpy  = API.new('_memccpy', 'PPIL', 'P', 'msvcrt')
         Memchr   = API.new('memchr', 'PIL', 'P', 'msvcrt')
         Memcmp   = API.new('memcmp', 'PPL', 'I', 'msvcrt')
         Memicmp  = API.new('_memicmp', 'PPL', 'I', 'msvcrt')
         Memmove  = API.new('memmove', 'PPL', 'P', 'msvcrt')
         Memset   = API.new('memset', 'PLL', 'L', 'msvcrt')
         Swab     = API.new('_swab', 'PPI', 'V', 'msvcrt')
         
         MemcpyPLL  = API.new('memcpy', 'PLL', 'P', 'msvcrt')
         MemcpyLPL  = API.new('memcpy', 'LPL', 'P', 'msvcrt')
         MemcpyLLL  = API.new('memcpy', 'LLL', 'P', 'msvcrt')
         MemcpyPPL  = API.new('memcpy', 'PPL', 'P', 'msvcrt')
         
         # Wrapper for the memcpy() function.  Both the +dest+ and +src+ can
         # be either a string or a memory address.  If +size+ is omitted, it
         # defaults to the length of +src+.
         # 
         def memcpy(dest, src, size = src.length)
            if dest.is_a?(Integer)
               if src.is_a?(String)
                  MemcpyLPL.call(dest, src, size)
               else
                  MemcpyLLL.call(dest, src, size)
               end
            else
               if src.is_a?(String)
                  MemcpyPPL.call(dest, src, size)
               else
                  MemcpyPLL.call(dest, src, size)
               end
            end
         end
      
         def memccpy(dest, src, char, count)
            Memccpy.call(dest, src, char, count)
         end
         
         def memchr(buf, char, count)
            Memchr.call(buf, char, count)
         end
         
         def memcmp(buf1, buf2, count)
            Memcmp.call(buf1, buf2, count)
         end
         
         def memicmp(buf1, buf2, count)
            Memicmp.call(buf1, buf2, count)
         end
         
         def memmove(dest, src, count)
            Memmove.call(dest, src, count)
         end
      
         def memset(dest, char, count)
            Memset.call(dest, char, count)
         end
         
         def swab(src, dest, count)
            Swab.call(src, dest, count)
         end
      end
   end
end
