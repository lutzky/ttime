require 'windows/api'
include Windows

module Windows
   module FileSystem
      API.auto_namespace = 'Windows::FileSystem'
      API.auto_constant  = true
      API.auto_method    = true
      API.auto_unicode   = true

      API.new('GetDiskFreeSpace', 'PPPPP', 'B')
      API.new('GetDiskFreeSpaceEx', 'PPPP', 'B')
   end
end
