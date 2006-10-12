require 'yaml'
require 'pp'

pp File.open(ARGV[0]) { |yf| YAML.load(yf) }
