require 'windows/api'
include Windows

module Windows
   module EventLog
      API.auto_namespace = 'Windows::EventLog'
      API.auto_constant  = true
      API.auto_method    = true
      API.auto_unicode   = true

      EVENTLOG_SEQUENTIAL_READ = 0x0001
      EVENTLOG_SEEK_READ       = 0x0002
      EVENTLOG_FORWARDS_READ   = 0x0004
      EVENTLOG_BACKWARDS_READ  = 0x0008

      EVENTLOG_SUCCESS          = 0x0000
      EVENTLOG_ERROR_TYPE       = 0x0001
      EVENTLOG_WARNING_TYPE     = 0x0002
      EVENTLOG_INFORMATION_TYPE = 0x0004
      EVENTLOG_AUDIT_SUCCESS    = 0x0008
      EVENTLOG_AUDIT_FAILURE    = 0x0010
      
      EVENTLOG_FULL_INFO = 0

      API.new('BackupEventLog', 'LP', 'B', 'advapi32')
      API.new('ClearEventLog', 'LP', 'B', 'advapi32')
      API.new('CloseEventLog', 'L', 'B', 'advapi32')
      API.new('DeregisterEventSource', 'L', 'B', 'advapi32')
      API.new('GetEventLogInformation', 'LLPLP', 'B', 'advapi32')
      API.new('GetNumberOfEventLogRecords', 'LP', 'B', 'advapi32')
      API.new('GetOldestEventLogRecord', 'LP', 'B', 'advapi32')
      API.new('NotifyChangeEventLog', 'LL', 'B', 'advapi32')
      API.new('OpenBackupEventLog', 'PP', 'L', 'advapi32')
      API.new('OpenEventLog', 'PP', 'L', 'advapi32')
      API.new('ReadEventLog', 'LLLPLPP', 'B', 'advapi32')
      API.new('RegisterEventSource', 'PP', 'L', 'advapi32')
      API.new('ReportEvent', 'LIILPILPP', 'B', 'advapi32')
   end
end
