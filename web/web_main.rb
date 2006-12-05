require "web_hook_manager"
include TTime::Web

hm = HookManager.new
hm.start_server

