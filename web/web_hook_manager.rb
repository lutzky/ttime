require 'webrick'
require 'yaml'


module TTime
  module Web
    class HookManager
      include WEBrick

      def initialize(config = {})
        config.update(:Port => 1984)
        @server = WEBrick::HTTPServer.new(config)
        @hooks = {'/test/nil' => {}}
      end

      def start_server()
        hooked_servlet = Class.new(HookServlet)
        hook_list = @hooks

        hooked_servlet.class_eval do
          @@hooks = hook_list.dup

          def hooks
            @@hooks
          end
        end
       
        @server.mount('/test/', hooked_servlet)
        @server.start()
      end

      def stop_server()
        @server.shutdown
      end

      def hook(hook,&target)
        @hooks[hook] = target
      end
    end

    class HookServlet < WEBrick::HTTPServlet::AbstractServlet
      def do_GET(req,resp)
        # the method hooks should be implemented by a class extending 
        # this one, otherwise this will fail.
        # I reccomend using the class_eval method for this porpouse
        hooks[req.request_uri].call(req,resp)
      end

      alias do_POST do_GET

    end
  end
end



