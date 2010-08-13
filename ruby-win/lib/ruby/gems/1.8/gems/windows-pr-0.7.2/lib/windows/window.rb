require 'windows/api'
include Windows

# See WinUser.h

module Windows
   module Window
      API.auto_namespace = 'Windows::Window'
      API.auto_constant  = true
      API.auto_method    = true
      API.auto_unicode   = false

      # ShowWindow() constants
      SW_HIDE             = 0
      SW_SHOWNORMAL       = 1
      SW_NORMAL           = 1
      SW_SHOWMINIMIZED    = 2
      SW_SHOWMAXIMIZED    = 3
      SW_MAXIMIZE         = 3
      SW_SHOWNOACTIVATE   = 4
      SW_SHOW             = 5
      SW_MINIMIZE         = 6
      SW_SHOWMINNOACTIVE  = 7
      SW_SHOWNA           = 8
      SW_RESTORE          = 9
      SW_SHOWDEFAULT      = 10
      SW_FORCEMINIMIZE    = 11
      SW_MAX              = 11
      
      API.new('GetClientRect', 'LP', 'B', 'user32')
      API.new('GetForegroundWindow', 'V', 'L', 'user32')
      API.new('GetWindowRect', 'LP', 'B', 'user32')
   end
end
