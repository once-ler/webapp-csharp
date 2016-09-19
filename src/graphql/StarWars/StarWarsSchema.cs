using System;
using GraphQL.Types;

namespace GraphQL.StarWars {
  public class StarWarsSchema : Schema {
    public StarWarsSchema() { }
    public StarWarsSchema(Func<Type, GraphType> resolveType)
      : base(resolveType) {
      Query = (ObjectGraphType)resolveType(typeof(StarWarsQuery));
    }
  }
}
