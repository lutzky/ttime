require 'test/unit'
require 'gettext/parser/ruby'
require 'gettext/parser/glade'
require 'gettext/parser/erb'

require 'rubygems'
require 'gettext/active_record'
require 'gettext/parser/active_record'
require 'active_record'

require 'gettext/rgettext'

begin
  `rake dropdb`
rescue
end
begin
  `rake createdb`
rescue
end


class TestGetTextParser < Test::Unit::TestCase
  def test_ruby
    ary = GetText::RubyParser.parse('test_rubyparser.rb')

    assert_equal(['aaa', 'test_rubyparser.rb:8'], ary[0])
    assert_equal(['aaa\n', 'test_rubyparser.rb:12'], ary[1])
    assert_equal(['bbb\nccc', 'test_rubyparser.rb:16'], ary[2])
    assert_equal(['bbb\nccc\nddd\n', 'test_rubyparser.rb:20'], ary[3])
    assert_equal(['eee', 'test_rubyparser.rb:27', 'test_rubyparser.rb:31'], ary[4])
    assert_equal(['fff', 'test_rubyparser.rb:31'], ary[5])
    assert_equal(['ggghhhiii', 'test_rubyparser.rb:35'], ary[6])
    assert_equal(['a"b"c"', 'test_rubyparser.rb:41'], ary[7])
    assert_equal(['d"e"f"', 'test_rubyparser.rb:45'], ary[8])
    assert_equal(['jjj', 'test_rubyparser.rb:49'], ary[9])
    assert_equal(['kkk', 'test_rubyparser.rb:50'], ary[10])
    assert_equal(['lllmmm', 'test_rubyparser.rb:54'], ary[11])
    assert_equal(['nnn\nooo', 'test_rubyparser.rb:62'], ary[12])
    assert_equal(["\#", 'test_rubyparser.rb:66', 'test_rubyparser.rb:70'], ary[13])
    assert_equal(["\\taaa", 'test_rubyparser.rb:74'], ary[14])
    assert_equal(["Here document1\\nHere document2\\n", 'test_rubyparser.rb:78'], ary[15])
#    assert_equal(["in_quote", 'test_rubyparser.rb:96'], ary[16])
  end

  def test_ruby_N
    ary = GetText::RubyParser.parse('test_rubyparser_N.rb')

    assert_equal(['aaa', 'test_rubyparser_N.rb:8'], ary[0])
    assert_equal(['aaa\n', 'test_rubyparser_N.rb:12'], ary[1])
    assert_equal(['bbb\nccc', 'test_rubyparser_N.rb:16'], ary[2])
    assert_equal(['bbb\nccc\nddd\n', 'test_rubyparser_N.rb:20'], ary[3])
    assert_equal(['eee', 'test_rubyparser_N.rb:27', 'test_rubyparser_N.rb:31'], ary[4])
    assert_equal(['fff', 'test_rubyparser_N.rb:31'], ary[5])
    assert_equal(['ggghhhiii', 'test_rubyparser_N.rb:35'], ary[6])
    assert_equal(['a"b"c"', 'test_rubyparser_N.rb:41'], ary[7])
    assert_equal(['d"e"f"', 'test_rubyparser_N.rb:45'], ary[8])
    assert_equal(['jjj', 'test_rubyparser_N.rb:49'], ary[9])
    assert_equal(['kkk', 'test_rubyparser_N.rb:50'], ary[10])
    assert_equal(['lllmmm', 'test_rubyparser_N.rb:54'], ary[11])
    assert_equal(['nnn\nooo', 'test_rubyparser_N.rb:62'], ary[12])
  end

  def test_ruby_n
    ary = GetText::RubyParser.parse('test_rubyparser_n_.rb')
    assert_equal(["aaa\000aaa2", 'test_rubyparser_n_.rb:8'], ary[0])
    assert_equal(["bbb\\n\000ccc2\\nccc2", 'test_rubyparser_n_.rb:12'], ary[1])
    assert_equal(["ddd\\nddd\000ddd2\\nddd2", 'test_rubyparser_n_.rb:16'], ary[2])
    assert_equal(["eee\\neee\\n\000eee2\\neee2\\n", 'test_rubyparser_n_.rb:21'], ary[3])
    assert_equal(["ddd\\neee\\n\000ddd\\neee2", 'test_rubyparser_n_.rb:27'], ary[4])
    assert_equal(["fff\000fff2", 'test_rubyparser_n_.rb:34', 'test_rubyparser_n_.rb:38'], ary[5])
    assert_equal(["ggg\000ggg2", 'test_rubyparser_n_.rb:38'], ary[6])
    assert_equal(["ggghhhiii\000jjjkkklll", 'test_rubyparser_n_.rb:42'], ary[7])
    assert_equal(["a\"b\"c\"\000a\"b\"c\"2", 'test_rubyparser_n_.rb:51'], ary[8])
    assert_equal(["mmmmmm\000mmm2mmm2", 'test_rubyparser_n_.rb:59'], ary[10])
    assert_equal(["nnn\000nnn2", 'test_rubyparser_n_.rb:60'], ary[11])
  end

  def test_glade
    ary = GetText::GladeParser.parse('test_gladeparser.glade')

    assert_equal(['window1', 'test_gladeparser.glade:8'], ary[0])
    assert_equal(['normal text', 'test_gladeparser.glade:29'], ary[1])
    assert_equal(['1st line\n2nd line\n3rd line', 'test_gladeparser.glade:50'], ary[2])
    assert_equal(['<span color="red" weight="bold" size="large">markup </span>', 'test_gladeparser.glade:73'], ary[3])
    assert_equal(['<span color="red">1st line markup </span>\n<span color="blue">2nd line markup</span>', 'test_gladeparser.glade:94'], ary[4])
    assert_equal(['<span>&quot;markup&quot; with &lt;escaped strings&gt;</span>', 'test_gladeparser.glade:116'], ary[5])
    assert_equal(['duplicated', 'test_gladeparser.glade:137', 'test_gladeparser.glade:158'], ary[6])
  end

  def test_erb
    ary = GetText::ErbParser.parse('test_erb.rhtml')

    assert_equal(['aaa', 'test_erb.rhtml:8'], ary[0])
    assert_equal(['aaa\n', 'test_erb.rhtml:11'], ary[1])
    assert_equal(['bbb', 'test_erb.rhtml:12'], ary[2])
    assert_equal(["ccc1\000ccc2", 'test_erb.rhtml:13'], ary[3])
  end


  def test_active_record
    GetText::ActiveRecordParser.init(
                                     :adapter  => "mysql",
                                     :username => "root",
                                     :password => "",
                                     :encoding => "utf8",
                                     :socket => "/var/lib/mysql/mysql.sock",
                                     :database => 'activerecord_unittest'
                                     )
    GetText::ActiveRecordParser.target?("fixtures/topic.rb")
    ary = GetText::ActiveRecordParser.parse("fixtures/topic.rb")
    assert_equal(14, ary.size)
    assert_equal(["topic", "fixtures/topic.rb:-"], ary[0])
    assert_equal(["Topic|Title", "fixtures/topic.rb:-"], ary[1])
    assert_equal(["Topic|Author name", "fixtures/topic.rb:-"], ary[2])
    assert_equal(["Topic|Author email address", "fixtures/topic.rb:-"], ary[3])
    assert_equal(["Topic|Written on", "fixtures/topic.rb:-"], ary[4])
    assert_equal(["Topic|Bonus time", "fixtures/topic.rb:-"], ary[5])
    assert_equal(["Topic|Last read", "fixtures/topic.rb:-"], ary[6])
    assert_equal(["Topic|Content", "fixtures/topic.rb:-"], ary[7])
    assert_equal(["Topic|Approved", "fixtures/topic.rb:-"], ary[8])
    assert_equal(["Topic|Replies count", "fixtures/topic.rb:-"], ary[9])
    assert_equal(["Topic|Parent", "fixtures/topic.rb:-"], ary[10])
    assert_equal(["Topic|Type", "fixtures/topic.rb:-"], ary[11])
    assert_equal(["Topic|Terms of service", "fixtures/topic.rb:5"], ary[12])
    assert_equal(["must be abided", "fixtures/topic.rb:6"], ary[13])

    GetText::ActiveRecordParser.target?("fixtures/reply.rb")
    ary = GetText::ActiveRecordParser.parse("fixtures/reply.rb")
    ary.sort!{|a, b| a[0].downcase <=> b[0].downcase}

    assert_equal(30, ary.size)
    assert_equal(["Empty", "fixtures/reply.rb:16", "fixtures/reply.rb:20"], ary[0])
    assert_equal(["is Content Mismatch", "fixtures/reply.rb:25"], ary[1])
    assert_equal(["is Wrong Create", "fixtures/reply.rb:30"], ary[2])
    assert_equal(["is Wrong Update", "fixtures/reply.rb:34"], ary[3])
    assert_equal(["reply", "fixtures/reply.rb:-"], ary[4])
    assert_equal(["Reply|Approved", "fixtures/reply.rb:-"], ary[5])
    assert_equal(["Reply|Author email address", "fixtures/reply.rb:-"], ary[6])
    assert_equal(["Reply|Author name", "fixtures/reply.rb:-"], ary[7])
    assert_equal(["Reply|Bonus time", "fixtures/reply.rb:-"], ary[8])
    assert_equal(["Reply|Content", "fixtures/reply.rb:-"], ary[9])
    assert_equal(["Reply|Last read", "fixtures/reply.rb:-"], ary[10])
    assert_equal(["Reply|Parent", "fixtures/reply.rb:-"], ary[11])
    assert_equal(["Reply|Replies count", "fixtures/reply.rb:-"], ary[12])
    assert_equal(["Reply|Title", "fixtures/reply.rb:-"], ary[13])
    assert_equal(["Reply|Topic", "fixtures/reply.rb:4"], ary[14])
    assert_equal(["Reply|Type", "fixtures/reply.rb:-"], ary[15])
    assert_equal(["Reply|Written on", "fixtures/reply.rb:-"], ary[16])

    assert_equal(["sillyreply", "fixtures/reply.rb:-"], ary[17])
    assert_equal(["SillyReply|Approved", "fixtures/reply.rb:-"], ary[18])
    assert_equal(["SillyReply|Author email address", "fixtures/reply.rb:-"], ary[19])
    assert_equal(["SillyReply|Author name", "fixtures/reply.rb:-"], ary[20])
    assert_equal(["SillyReply|Bonus time", "fixtures/reply.rb:-"], ary[21])
    assert_equal(["SillyReply|Content", "fixtures/reply.rb:-"], ary[22])
    assert_equal(["SillyReply|Last read", "fixtures/reply.rb:-"], ary[23])
    assert_equal(["SillyReply|Parent", "fixtures/reply.rb:-"], ary[24])
    assert_equal(["SillyReply|Replies count", "fixtures/reply.rb:-"], ary[25])
    assert_equal(["SillyReply|Title", "fixtures/reply.rb:-"], ary[26])
    assert_equal(["SillyReply|Type", "fixtures/reply.rb:-"], ary[27])
    assert_equal(["SillyReply|Written on", "fixtures/reply.rb:-"], ary[28])

    assert_equal(["topic", "fixtures/reply.rb:-"], ary[29])

    GetText::ActiveRecordParser.target?("fixtures/book.rb")
    ary = GetText::ActiveRecordParser.parse("fixtures/book.rb")
    ary.sort!{|a, b| a[0].downcase <=> b[0].downcase}
    assert_equal(4, ary.size)
    assert_equal(["book", "fixtures/book.rb:-"], ary[0])
    assert_equal(["Book|Created at", "fixtures/book.rb:-"], ary[1])
    assert_equal(["Book|Price", "fixtures/book.rb:-"], ary[2])
    assert_equal(["Book|Updated at", "fixtures/book.rb:-"], ary[3])

    GetText::ActiveRecordParser.target?("fixtures/user.rb")
    ary = GetText::ActiveRecordParser.parse("fixtures/user.rb")
    assert_equal(0, ary.size)
  end


  def test_rgettext_parse
    GetText::ErbParser.init(:extnames => ['.rhtml', '.rxml'])
    ary = GetText::RGetText.parse('test_erb.rhtml')
    assert_equal(['aaa', 'test_erb.rhtml:8'], ary[0])
    assert_equal(['aaa\n', 'test_erb.rhtml:11'], ary[1])
    assert_equal(['bbb', 'test_erb.rhtml:12'], ary[2])
    assert_equal(["ccc1\000ccc2", 'test_erb.rhtml:13'], ary[3])

    ary = GetText::RGetText.parse('test_erb.rxml')
    assert_equal(['aaa', 'test_erb.rxml:9'], ary[0])
    assert_equal(['aaa\n', 'test_erb.rxml:12'], ary[1])
    assert_equal(['bbb', 'test_erb.rxml:13'], ary[2])
    assert_equal(["ccc1\000ccc2", 'test_erb.rxml:14'], ary[3])


    ary = GetText::RGetText.parse('test_rubyparser_n_.rb')
    assert_equal(["ooo\000ppp", 'test_rubyparser_n_.rb:64', 'test_rubyparser_n_.rb:65'], ary[12])
    assert_equal(["qqq\000rrr", 'test_rubyparser_n_.rb:69', 'test_rubyparser_n_.rb:70'], ary[13])
  end

end
