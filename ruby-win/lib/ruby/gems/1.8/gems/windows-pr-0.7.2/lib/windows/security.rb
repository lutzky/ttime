require 'windows/api'
include Windows

module Windows
   module Security
      API.auto_namespace = 'Windows::Security'
      API.auto_constant  = true
      API.auto_method    = true
      API.auto_unicode   = false

      ACL_REVISION                   = 2
      ACL_REVISION2                  = 2
      ACL_REVISION3                  = 3
      ACL_REVISION4                  = 4
      ALLOW_ACE_LENGTH               = 62
      DACL_SECURITY_INFORMATION      = 4
      SE_DACL_PRESENT                = 4
      SECURITY_DESCRIPTOR_MIN_LENGTH = 20
      SECURITY_DESCRIPTOR_REVISION   = 1
      
      SECURITY_NULL_SID_AUTHORITY         = 0
      SECURITY_WORLD_SID_AUTHORITY        = 1
      SECURITY_LOCAL_SID_AUTHORITY        = 2
      SECURITY_CREATOR_SID_AUTHORITY      = 3
      SECURITY_NON_UNIQUE_AUTHORITY       = 4
      SECURITY_NT_AUTHORITY               = 5
      SECURITY_RESOURCE_MANAGER_AUTHORITY = 9      
      
      GENERIC_RIGHTS_MASK = 4026597376
      GENERIC_RIGHTS_CHK  = 4026531840
      REST_RIGHTS_MASK    = 2097151
      
      API.new('AddAce', 'PLLLL', 'B', 'advapi32')
      API.new('CopySid', 'LLP', 'B', 'advapi32')
      API.new('ConvertSidToStringSid', 'PL', 'B', 'advapi32')
      API.new('ConvertSecurityDescriptorToStringSecurityDescriptor', 'PLLPP', 'B', 'advapi32')
      API.new('ConvertStringSecurityDescriptorToSecurityDescriptor', 'PLPP', 'B', 'advapi32')
      API.new('ConvertStringSidToSid', 'PL', 'B', 'advapi32')
      API.new('GetAce', 'LLP', 'B', 'advapi32')
      API.new('GetFileSecurity', 'PLPLP', 'B', 'advapi32')
      API.new('GetLengthSid', 'P', 'L', 'advapi32')
      API.new('GetSecurityDescriptorControl', 'PPP', 'B', 'advapi32')
      API.new('GetSecurityDescriptorDacl', 'PPPP', 'B', 'advapi32')
      API.new('GetSecurityDescriptorGroup', 'PPI', 'B', 'advapi32')
      API.new('GetSecurityDescriptorLength', 'P', 'L', 'advapi32')
      API.new('GetSecurityDescriptorOwner', 'PPI', 'B', 'advapi32')
      API.new('GetSecurityDescriptorRMControl', 'PP', 'L', 'advapi32')
      API.new('GetSecurityDescriptorSacl', 'PIPI', 'B', 'advapi32')
      API.new('InitializeAcl', 'PLL', 'B', 'advapi32')
      API.new('InitializeSecurityDescriptor', 'PL', 'B', 'advapi32')
      API.new('IsValidSecurityDescriptor', 'P', 'B', 'advapi32')
      API.new('LookupAccountName', 'PPPPPPP', 'B', 'advapi32')
      API.new('LookupAccountSid', 'PLPPPPP', 'B', 'advapi32')
      API.new('SetFileSecurity', 'PPP', 'B', 'advapi32') 
      API.new('SetSecurityDescriptorDacl', 'PIPI', 'B', 'advapi32')
      API.new('SetSecurityDescriptorGroup', 'PPI', 'B', 'advapi32')
      API.new('SetSecurityDescriptorOwner', 'PPI', 'B', 'advapi32')
      API.new('SetSecurityDescriptorRMControl', 'PP', 'L', 'advapi32')
      API.new('SetSecurityDescriptorSacl', 'PIPI', 'B', 'advapi32')
   end
end
