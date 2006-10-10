require 'logic/repy'

$DEBUG = false

a = TTime::Logic::Repy.new(open("data/REPY") { |f| f.read })

y a.hash

