=begin rdoc
= Nicknames

Some courses have very long names, and we'd like to to display them (or search
for them) using shortened names. You can override included nicknames by
creating a file in your home directory,
<tt>/home/youruser/.ttime/nicknames.txt</tt>, of the following format:

  Very Long course name Which is hard to remember
  MyCourse

  Another Very Long course name which couldn't possible fit in schedule
  Anothercourse

=end

require 'singleton'

module TTime::Logic
  class Nicknames
    include Singleton

    # Candidates for nickname paths are given either relative to $0's directory
    # or absolutely. They are processed in order, with later ones overriding
    # the former.
    NicknamePathCandidates = [
      '../data/ttime/',
      '/usr/share/ttime/',
      '/usr/local/share/ttime/',
      File::join(ENV["HOME"], ".ttime"),
    ]

    NicknameFile = "nicknames.txt"

    attr_reader :beautify

    def initialize
      @beautify = {}

      base_path = (Pathname.new $0).dirname
      NicknamePathCandidates.each do |p|
        current_nickname_file = base_path + p + NicknameFile
        if current_nickname_file.readable?
          load_nicknames_from(current_nickname_file)
        end
      end
    end

    def load_nicknames_from nickname_file
      line_pairs = open(nickname_file, "r") { |f| f.read.split("\n\n") }
      line_pairs.each do |line_pair|
        ugly_version, beautiful_version = line_pair.split("\n")
        @beautify[ugly_version] = beautiful_version
      end
    end
  end
end
