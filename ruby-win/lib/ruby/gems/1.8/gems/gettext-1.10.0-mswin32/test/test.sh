#!/bin/sh

export LC_ALL="ja_JP.eucJP"; 
export LANG="ja_JP.eucJP"; 
rake rebuilddb
ruby  -I../lib -I../ext/gettext gettext_runner.rb $1
ruby  -I../lib -I../ext/gettext gettext_test_multi_textdomain.rb
ruby -I../lib -I../ext/gettext gettext_test_cgi.rb
ruby -I../lib -I../ext/gettext gettext_test_rails.rb
ruby -I../lib -I../ext/gettext gettext_test_rails_caching.rb
ruby -I../lib -I../ext/gettext gettext_test_active_record.rb
cd rails
rake -I../../lib -I../../ext/gettext test
cd ..
