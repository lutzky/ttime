Gem::Specification.new do |s|
  s.name = %q{win32-dir}
  s.version = "0.3.2"
  s.date = %q{2007-07-25}
  s.summary = %q{Extra constants and methods for the Dir class on Windows.}
  s.email = %q{djberg96@gmail.com}
  s.homepage = %q{http://www.rubyforge.org/projects/win32utils}
  s.description = %q{Extra constants and methods for the Dir class on Windows.}
  s.has_rdoc = true
  s.authors = ["Daniel J. Berger"]
  s.files = ["lib/win32/dir.rb", "test/CVS", "test/tc_dir.rb", "CHANGES", "CVS", "examples", "lib", "MANIFEST", "Rakefile", "README", "test", "win32-dir.gemspec"]
  s.test_files = ["test/tc_dir.rb"]
  s.extra_rdoc_files = ["README", "CHANGES", "MANIFEST"]
  s.add_dependency(%q<windows-pr>, [">= 0.5.1"])
end
