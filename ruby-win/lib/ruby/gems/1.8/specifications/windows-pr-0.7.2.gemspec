Gem::Specification.new do |s|
  s.name = %q{windows-pr}
  s.version = "0.7.2"
  s.date = %q{2007-10-15}
  s.summary = %q{Windows functions and constants bundled via Win32::API}
  s.email = %q{djberg96@gmail.com}
  s.homepage = %q{http://www.rubyforge.org/projects/win32utils}
  s.description = %q{Windows functions and constants bundled via Win32::API}
  s.has_rdoc = true
  s.required_ruby_version = Gem::Version::Requirement.new(">= 0")
  s.cert_chain = []
  s.authors = ["Daniel J. Berger"]
  s.files = ["doc/conversion_guide.txt", "lib/windows/clipboard.rb", "lib/windows/com.rb", "lib/windows/console.rb", "lib/windows/debug.rb", "lib/windows/device_io.rb", "lib/windows/directory.rb", "lib/windows/error.rb", "lib/windows/eventlog.rb", "lib/windows/file.rb", "lib/windows/filesystem.rb", "lib/windows/file_mapping.rb", "lib/windows/handle.rb", "lib/windows/library.rb", "lib/windows/limits.rb", "lib/windows/memory.rb", "lib/windows/national.rb", "lib/windows/network_management.rb", "lib/windows/nio.rb", "lib/windows/path.rb", "lib/windows/pipe.rb", "lib/windows/process.rb", "lib/windows/registry.rb", "lib/windows/security.rb", "lib/windows/service.rb", "lib/windows/shell.rb", "lib/windows/sound.rb", "lib/windows/synchronize.rb", "lib/windows/system_info.rb", "lib/windows/time.rb", "lib/windows/unicode.rb", "lib/windows/volume.rb", "lib/windows/window.rb", "test/tc_clipboard.rb", "test/tc_com.rb", "test/tc_console.rb", "test/tc_debug.rb", "test/tc_error.rb", "test/tc_eventlog.rb", "test/tc_file.rb", "test/tc_library.rb", "test/tc_memory.rb", "test/tc_msvcrt_buffer.rb", "test/tc_path.rb", "test/tc_pipe.rb", "test/tc_process.rb", "test/tc_registry.rb", "test/tc_security.rb", "test/tc_synchronize.rb", "test/tc_unicode.rb", "test/tc_volume.rb", "test/tc_window.rb", "lib/windows/msvcrt/buffer.rb", "lib/windows/msvcrt/directory.rb", "lib/windows/msvcrt/file.rb", "lib/windows/msvcrt/io.rb", "lib/windows/msvcrt/string.rb", "lib/windows/msvcrt/time.rb", "lib/windows/gdi/bitmap.rb", "lib/windows/gdi/device_context.rb", "lib/windows/gdi/painting_drawing.rb", "test/CVS", "CHANGES", "CVS", "doc", "lib", "MANIFEST", "Rakefile", "README", "test", "windows-pr.gemspec"]
  s.test_files = ["test/tc_clipboard.rb", "test/tc_com.rb", "test/tc_console.rb", "test/tc_debug.rb", "test/tc_error.rb", "test/tc_eventlog.rb", "test/tc_file.rb", "test/tc_library.rb", "test/tc_memory.rb", "test/tc_msvcrt_buffer.rb", "test/tc_path.rb", "test/tc_pipe.rb", "test/tc_process.rb", "test/tc_registry.rb", "test/tc_security.rb", "test/tc_synchronize.rb", "test/tc_unicode.rb", "test/tc_volume.rb", "test/tc_window.rb"]
  s.extra_rdoc_files = ["MANIFEST", "README", "CHANGES"]
  s.add_dependency(%q<windows-api>, [">= 0.2.0"])
end
