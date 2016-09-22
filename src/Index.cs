using System.Collections.Generic;
using webserver.host;
using webserver.handlers;
// Test GraphQL Schema
using GraphQL.Types;
using GraphQL.StarWars;
using GraphQL.StarWars.Types;

namespace webserver {
  class index {

    static void Main(string[] args) {
      List<IRequestHandler> _handlers = new List<IRequestHandler>();
      // add your custom handlers
      // _handlers.Add(new webserver.handlers.YourCustomHandler());
      // Add GraphQL handler, you are responsible for creating the GraphQL Schema
      _handlers.Add(new GraphQLHandler(new StarWarsSchema{ Query = new StarWarsQuery(new StarWarsData()) }));
      // _handlers.Add(new GraphQLHandler(new StarWarsSchema(type => new StarWarsQuery())));
      
      // create the async server
      AsyncHttpListener server = new AsyncHttpListener(7777, _handlers);
      server.Start();
    }

  }
}