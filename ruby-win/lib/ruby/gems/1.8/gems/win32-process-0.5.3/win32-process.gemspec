require "rubygems"

spec = Gem::Specification.new do |gem|
   gem.name        = "win32-process"
   gem.version     = "0.5.3"
   gem.author      = "Daniel J. Berger"
   gem.email       = "djberg96@gmail.com"
   gem.homepage    = "http://www.rubyforge.org/projects/win32utils"
   gem.platform    = Gem::Platform::RUBY
   gem.summary     = "Adds fork, wait, wait2, waitpid, waitpid2 and a special kill method"
   gem.description = "Adds fork, wait, wait2, waitpid, waitpid2 and a special kill method"
   gem.test_file   = "test/tc_process.rb"
   gem.has_rdoc    = true
   gem.files       = Dir["lib/win32/*.rb"] + Dir["test/*"] + Dir["[A-Z]*"]
   gem.files.reject! { |fn| fn.include? "CVS" }
   gem.require_path = "lib"
   gem.extra_rdoc_files = ["README", "CHANGES", "MANIFEST"]
   gem.add_dependency("windows-pr", ">= 0.5.2")
end

if $0 == __FILE__
   Gem.manage_gems
   Gem::Builder.new(spec).build
end
