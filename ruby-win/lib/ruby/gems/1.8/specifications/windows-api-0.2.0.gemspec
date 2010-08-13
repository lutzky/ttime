Gem::Specification.new do |s|
  s.name = %q{windows-api}
  s.version = "0.2.0"
  s.date = %q{2007-09-20}
  s.summary = %q{An easier way to create methods using Win32API}
  s.email = %q{djberg96@gmail.com}
  s.homepage = %q{http://www.rubyforge.org/projects/win32utils}
  s.description = %q{An easier way to create methods using Win32API}
  s.has_rdoc = true
  s.authors = ["Daniel J. Berger"]
  s.files = ["lib/windows/api.rb", "test/CVS", "test/tc_windows_api.rb", "CHANGES", "CVS", "lib", "MANIFEST", "Rakefile", "README", "test", "windows-api.gemspec"]
  s.test_files = ["test/tc_windows_api.rb"]
  s.extra_rdoc_files = ["README", "CHANGES", "MANIFEST"]
  s.add_dependency(%q<win32-api>, [">= 1.0.0"])
end
