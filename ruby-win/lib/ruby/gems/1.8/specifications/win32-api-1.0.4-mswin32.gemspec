Gem::Specification.new do |s|
  s.name = %q{win32-api}
  s.version = "1.0.4"
  s.date = %q{2007-10-29}
  s.summary = %q{A superior replacement for Win32API}
  s.email = %q{djberg96@gmail.com}
  s.homepage = %q{http://www.rubyforge.org/projects/win32utils}
  s.rubyforge_project = %q{win32utils}
  s.description = %q{A superior replacement for Win32API}
  s.has_rdoc = true
  s.required_ruby_version = Gem::Version::Requirement.new(">= 1.8.0")
  s.platform = %q{mswin32}
  s.authors = ["Daniel J. Berger"]
  s.files = ["ext/extconf.rb", "ext/win32", "ext/win32/api.c", "test/tc_win32_api.rb", "test/tc_win32_api_callback.rb", "lib/win32/api.so", "README", "CHANGES", "MANIFEST"]
  s.test_files = ["test/tc_win32_api.rb", "test/tc_win32_api_callback.rb"]
  s.extra_rdoc_files = ["README", "CHANGES", "MANIFEST", "ext/win32/api.c"]
end
