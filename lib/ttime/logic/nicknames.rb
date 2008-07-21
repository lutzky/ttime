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
        puts "'#{ugly_version}' -> #{beautiful_version}" 
        @beautify[ugly_version] = beautiful_version
      end
    end
  end
end
