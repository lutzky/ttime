require 'windows/api'
include Windows

module Windows
   module MSVCRT
      module String
         API.auto_constant = false
         API.auto_method   = false
         API.auto_unicode  = false

         Strcmp = API.new('strcmp', 'PP', 'I', 'msvcrt')
         Strcpy = API.new('strcpy', 'PL', 'L', 'msvcrt')
         Strlen = API.new('strlen', 'P', 'L', 'msvcrt')
         Strrev = API.new('_strrev', 'P', 'P', 'msvcrt')
        
         Mbscmp = API.new('_mbscmp', 'PP', 'I', 'msvcrt')
         Mbscpy = API.new('_mbscpy', 'PL', 'L', 'msvcrt')
         Mbslen = API.new('_mbslen', 'P', 'L', 'msvcrt')
         Mbsrev = API.new('_mbsrev', 'P', 'P', 'msvcrt')
         
         Wcscmp = API.new('wcscmp', 'PP', 'I', 'msvcrt')
         Wcscpy = API.new('wcscpy', 'PL', 'L', 'msvcrt')
         Wcslen = API.new('wcslen', 'P', 'L', 'msvcrt')
         Wcsrev = API.new('_wcsrev', 'P', 'P', 'msvcrt')

         def strcmp(str1, str2)
            if str1 == 0 || str2 == 0
               return nil
            end
            Strcmp.call(str1, str2)
         end
         
         def strcpy(dest, src)
            return nil if src == 0
            Strcpy.call(dest, src)
         end
         
         def strlen(string)
            return nil if string == 0
            Strlen.call(string)
         end
         
         def strrev(str)
            return nil if str == 0
            Strrev.call(str)
         end
         
         def mbscmp(str1, str2)
            if str1 == 0 || str2 == 0
               return nil
            end
            Mbscmp.call(str1, str2)
         end
         
         def mbscpy(dest, src)
            return nil if src == 0
            Mbscpy.call(dest, src)
         end
         
         def mbslen(string)
            return nil if string == 0
            Mbslen.call(string)
         end
         
         def mbsrev(str)
            return nil if str == 0
            Mbsrev.call(str)
         end
         
         def wcscmp(str1, str2)
            if str1 == 0 || str2 == 0
               return nil
            end
            Wcscmp.call(str1, str2)
         end

         def wcscpy(dest, src)
            return nil if src == 0
            Wcscpy.call(dest, src)
         end
         
         def wcslen(string)
            return nil if string == 0
            Wcslen.call(string)
         end
         
         def wcsrev(str)
            return nil if str == 0
            Wcsrev.call(str)
         end  
      end
   end
end
