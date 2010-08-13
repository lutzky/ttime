$LOAD_PATH.unshift(Dir.pwd)
$LOAD_PATH.unshift(File.join(Dir.pwd, 'test'))

require 'tc_eventlog'
require 'tc_mc'
