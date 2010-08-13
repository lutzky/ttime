require 'windows/api'
include Windows

module Windows
   module Process
      API.auto_namespace = 'Windows::Process'
      API.auto_constant  = true
      API.auto_method    = true
      API.auto_unicode   = true

      PROCESS_ALL_ACCESS                = 0x1F0FFF
      PROCESS_CREATE_PROCESS            = 0x0080
      PROCESS_CREATE_THREAD             = 0x0002
      PROCESS_DUP_HANDLE                = 0x0040
      PROCESS_QUERY_INFORMATION         = 0x0400
      PROCESS_QUERY_LIMITED_INFORMATION = 0x1000
      PROCESS_SET_QUOTA                 = 0x0100
      PROCESS_SET_INFORMATION           = 0x0200
      PROCESS_SUSPEND_RESUME            = 0x0800
      PROCESS_TERMINATE                 = 0x0001
      PROCESS_VM_OPERATION              = 0x0008
      PROCESS_VM_READ                   = 0x0010
      PROCESS_VM_WRITE                  = 0x0020
      SYNCHRONIZE                       = 1048576
      STILL_ACTIVE                      = 259
      
      ABOVE_NORMAL_PRIORITY_CLASS = 0x00008000
      BELOW_NORMAL_PRIORITY_CLASS = 0x00004000
      HIGH_PRIORITY_CLASS         = 0x00000080
      IDLE_PRIORITY_CLASS         = 0x00000040
      NORMAL_PRIORITY_CLASS       = 0x00000020
      REALTIME_PRIORITY_CLASS     = 0x00000100
      
      # Process creation flags
      CREATE_BREAKAWAY_FROM_JOB        = 0x01000000
      CREATE_DEFAULT_ERROR_MODE        = 0x04000000
      CREATE_NEW_CONSOLE               = 0x00000010
      CREATE_NEW_PROCESS_GROUP         = 0x00000200
      CREATE_NO_WINDOW                 = 0x08000000
      CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000
      CREATE_SEPARATE_WOW_VDM          = 0x00000800
      CREATE_SHARED_WOW_VDM            = 0x00001000
      CREATE_SUSPENDED                 = 0x00000004
      CREATE_UNICODE_ENVIRONMENT       = 0x00000400
      DEBUG_ONLY_THIS_PROCESS          = 0x00000002
      DEBUG_PROCESS                    = 0x00000001
      DETACHED_PROCESS                 = 0x00000008
      
      STARTF_USESHOWWINDOW    = 0x00000001
      STARTF_USESIZE          = 0x00000002
      STARTF_USEPOSITION      = 0x00000004
      STARTF_USECOUNTCHARS    = 0x00000008
      STARTF_USEFILLATTRIBUTE = 0x00000010
      STARTF_RUNFULLSCREEN    = 0x00000020
      STARTF_FORCEONFEEDBACK  = 0x00000040
      STARTF_FORCEOFFFEEDBACK = 0x00000080
      STARTF_USESTDHANDLES    = 0x00000100
      STARTF_USEHOTKEY        = 0x00000200
      
      API.new('CreateProcess', 'LPLLLLLPPP', 'B')
      API.new('CreateRemoteThread', 'LPLLPLP', 'L')
      API.new('CreateThread', 'PLPPLP', 'L')
      API.new('ExitProcess', 'L', 'V')
      API.new('GetCommandLine', 'V', 'P')
      API.new('GetCurrentProcess', 'V', 'L')      
      API.new('GetCurrentProcessId', 'V', 'L')
      API.new('GetEnvironmentStrings', 'V', 'L')
      API.new('GetEnvironmentVariable', 'PPL', 'L')
      API.new('GetExitCodeProcess', 'LP', 'B')
      API.new('GetPriorityClass', 'L', 'L')
      API.new('GetProcessTimes', 'LPPPP', 'B')
      API.new('GetStartupInfo', 'P', 'V')
      API.new('OpenProcess', 'LIL', 'L')
      API.new('SetEnvironmentVariable', 'PP', 'I')
      API.new('Sleep', 'L', 'V')
      API.new('SleepEx', 'LI', 'L')
      API.new('TerminateProcess', 'LL', 'B')
      API.new('WaitForInputIdle', 'LL', 'L', 'user32')
      
      # Windows XP or later
      begin
         API.new('GetProcessId', 'L', 'L')
         API.new('GetProcessHandleCount', 'LP', 'B')
      rescue Exception
         # Do nothing - not supported on current platform.  It's up to you to
         # check for the existence of the constant in your code.
      end
   end
end
