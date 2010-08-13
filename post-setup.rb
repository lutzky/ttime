require 'rake'

rake_app = Rake.application
rake_app.init
rake_app.load_rakefile
rake_app.invoke_task :makemo
