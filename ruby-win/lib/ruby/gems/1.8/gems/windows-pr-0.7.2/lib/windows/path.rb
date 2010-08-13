require 'windows/api'
include Windows

module Windows
   module Path
      API.auto_namespace = 'Windows::Path'
      API.auto_constant  = true
      API.auto_method    = true
      API.auto_unicode   = true

      # These constants are for use by the PathGetCharType() function.
      GCT_INVALID   = 0x0000   # Character is not valid in a path.
      GCT_LFNCHAR   = 0x0001   # Character is valid in a long file name.
      GCT_SHORTCHAR = 0x0002   # Character is valid in a short (8.3) file name.
      GCT_WILD      = 0x0004   # Character is a wildcard character.
      GCT_SEPARATOR = 0x0008   # Character is a path separator.

      API.new('PathAddBackslash', 'P', 'P', 'shlwapi')
      API.new('PathAddExtension', 'PP', 'B', 'shlwapi')
      API.new('PathAppend', 'PP', 'B', 'shlwapi')
      API.new('PathBuildRoot', 'PI', 'P', 'shlwapi')
      API.new('PathCanonicalize', 'PP', 'B', 'shlwapi')
      API.new('PathCombine', 'PPP', 'P', 'shlwapi')
      API.new('PathCommonPrefix', 'PPP', 'I', 'shlwapi')
      API.new('PathCompactPath', 'PPI', 'B', 'shlwapi')
      API.new('PathCompactPathEx', 'PPIL', 'B', 'shlwapi')
      API.new('PathCreateFromUrl', 'PPPL', 'B', 'shlwapi')
      API.new('PathFileExists', 'P', 'B', 'shlwapi')
      API.new('PathFindExtension', 'P', 'P', 'shlwapi')
      API.new('PathFindFileName', 'P', 'P', 'shlwapi')
      API.new('PathFindNextComponent', 'P', 'P', 'shlwapi')
      API.new('PathFindOnPath', 'PP', 'B', 'shlwapi')
      API.new('PathFindSuffixArray', 'PPI', 'P', 'shlwapi')
      API.new('PathGetArgs', 'P', 'P', 'shlwapi')
      API.new('PathGetCharType', 'P', 'I', 'shlwapi')
      API.new('PathGetDriveNumber', 'P', 'I', 'shlwapi')
      API.new('PathIsContentType', 'PP', 'B', 'shlwapi')
      API.new('PathIsDirectory', 'P', 'B', 'shlwapi')
      API.new('PathIsDirectoryEmpty', 'P', 'B', 'shlwapi')
      API.new('PathIsFileSpec', 'P', 'B', 'shlwapi')
      API.new('PathIsLFNFileSpec', 'P', 'B', 'shlwapi')
      API.new('PathIsNetworkPath', 'P', 'B', 'shlwapi')
      API.new('PathIsPrefix', 'PP', 'B', 'shlwapi')
      API.new('PathIsRelative', 'P', 'B', 'shlwapi')
      API.new('PathIsRoot', 'P', 'B', 'shlwapi')
      API.new('PathIsSameRoot', 'PP', 'B', 'shlwapi')
      API.new('PathIsSystemFolder', 'PL', 'B', 'shlwapi')
      API.new('PathIsUNC', 'P', 'B', 'shlwapi')
      API.new('PathIsUNCServer', 'P', 'B', 'shlwapi')
      API.new('PathIsUNCServerShare', 'P', 'B', 'shlwapi')
      API.new('PathIsURL', 'P', 'B', 'shlwapi')
      API.new('PathMakePretty', 'P', 'B', 'shlwapi')
      API.new('PathMakeSystemFolder', 'P', 'B', 'shlwapi')
      API.new('PathMatchSpec', 'PP', 'B', 'shlwapi')
      API.new('PathParseIconLocation', 'P', 'I', 'shlwapi')
      API.new('PathQuoteSpaces', 'P', 'V', 'shlwapi')
      API.new('PathRelativePathTo', 'PPLPL', 'B', 'shlwapi')
      API.new('PathRemoveArgs', 'P', 'V', 'shlwapi')
      API.new('PathRemoveBackslash', 'P', 'P', 'shlwapi')
      API.new('PathRemoveBlanks', 'P', 'V', 'shlwapi')
      API.new('PathRemoveExtension', 'P','V', 'shlwapi')
      API.new('PathRemoveFileSpec', 'P', 'B', 'shlwapi')
      API.new('PathRenameExtension', 'PP', 'B', 'shlwapi')
      API.new('PathSearchAndQualify', 'PPI', 'B', 'shlwapi')
      API.new('PathSetDlgItemPath', 'LIP', 'V', 'shlwapi')
      API.new('PathSkipRoot', 'P', 'P', 'shlwapi')
      API.new('PathStripPath', 'P', 'V', 'shlwapi')
      API.new('PathStripToRoot', 'P', 'B', 'shlwapi')
      API.new('PathUndecorate', 'P', 'V', 'shlwapi')
      API.new('PathUnExpandEnvStrings', 'PPI', 'B', 'shlwapi')
      API.new('PathUnmakeSystemFolder', 'P', 'B', 'shlwapi')
      API.new('PathUnquoteSpaces', 'P', 'V', 'shlwapi')
   end
end
