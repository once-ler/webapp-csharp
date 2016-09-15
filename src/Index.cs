using System.Collections.Generic;
using webserver.host;
using webserver.handlers;

namespace webserver {
  class index {

    static void Main2(string[] args) {
      List<IRequestHandler> _handlers = new List<IRequestHandler>();
      // add your custom handlers
      // _handlers.Add(new webserver.handlers.YourCustomHandler());
      // create the async server
      AsyncHttpListener server = new AsyncHttpListener(7777, _handlers);
      server.Start();
    }

  }
}