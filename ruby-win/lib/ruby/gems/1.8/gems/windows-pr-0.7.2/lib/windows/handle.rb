require 'windows/api'
include Windows

module Windows
   module Handle
      API.auto_namespace = 'Windows::Handle'
      API.auto_constant  = false # Due to some functions with leading '_'. 
      API.auto_method    = false
      API.auto_unicode   = false

      INVALID_HANDLE_VALUE           = -1
      HANDLE_FLAG_INHERIT            = 0x00000001
      HANDLE_FLAG_PROTECT_FROM_CLOSE = 0x00000002
      
      CloseHandle          = API.new('CloseHandle', 'L', 'I')
      DuplicateHandle      = API.new('DuplicateHandle', 'LLLLLIL', 'I')
      GetHandleInformation = API.new('GetHandleInformation', 'LL', 'I')
      SetHandleInformation = API.new('SetHandleInformation', 'LLL', 'I')
      GetOSFHandle         = API.new('_get_osfhandle', 'I', 'L', 'msvcrt')
      OpenOSFHandle        = API.new('_open_osfhandle', 'LI', 'I', 'msvcrt')
            
      def CloseHandle(handle)
         CloseHandle.call(handle) != 0
      end
      
      def DuplicateHandle(sphandle, shandle, thandle, access, ihandle, opts)
         DuplicateHandle.call(sphandle, shandle, thandle, access, ihandle, opts) != 0
      end
      
      def GetHandleInformation(handle, flags)
         GetHandleInformation.call(handle, flags) != 0
      end
      
      def SetHandleInformation(handle, mask, flags)
         SetHandleInformation.call(handle, mask, flags) != 0
      end
      
      def get_osfhandle(fd)
         GetOSFHandle.call(fd)
      end
      
      def open_osfhandle(handle, flags)
         OpenOSFHandle.call(handle, flags)
      end
   end
end
