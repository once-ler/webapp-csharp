using GraphQL.StarWars.Types;
using GraphQL.Types;

namespace GraphQL.StarWars {
  public class StarWarsQuery : ObjectGraphType {
    /*
    // Tests
    localhost:7777/graphql?query={ droid(id: "4") { id name } }
    localhost:7777/graphql?query={ hero { id name } }
    localhost:7777/graphql?query={ hero { __typename name } }
    query IntrospectionDroidKindQuery {
                  __type(name: ""Droid"") {
                    name,
                    kind
                  }
                }
    query IntrospectionCharacterKindQuery {
              __type(name: ""Character"") {
                name
                kind
              }
            }
    
    query IntrospectionCharacterKindQuery {
              __type(name: ""Character"") {
                name
                kind
                  possibleTypes {
                    name,
                    kind
                  }
              }
            }
    query IntrospectionDroidFieldsQuery {
              __type(name: ""Droid"") {
                name
                fields {
                    name
                    type {
                        name
                        kind
                    }
                }
              }
            }
    query IntrospectionDroidDescriptionQuery {
              __type(name: ""Droid"") {
                name
                description
              }
            }
    query SchemaIntrospectionQuery {
              __schema {
                types { name, kind }
                queryType { name, kind }
                mutationType { name }
                directives {
                  name
                  description
                  onOperation
                  onFragment
                  onField
                }
              }
            }
    query SchemaIntrospectionQuery {
              __schema {
                queryType {
                  fields {
                    name
                    args {
                      name
                      description
                      type {
                        name
                        kind
                        ofType {
                          name
                          kind
                        }
                      }
                      defaultValue
                    }
                  }
                }
              }
            }
    
    query SomeDroids {
                  r2d2: droid(id: ""3"") {
                    ...DroidFragment
                  }
                  c3po: droid(id: ""4"") {
                    ...DroidFragment
                  }
               }
               fragment DroidFragment on Droid {
                 name
               }
    
    query SomeDroids {
                  r2d2: droid(id: ""3"") {
                    ... on Character {
                      name
                    }
                  }
               }
     
    query SomeDroids {
                  r2d2: droid(id: ""3"") {
                    ... {
                      name
                    }
                  }
               }
    */
    public StarWarsQuery() {
    }

    public StarWarsQuery(StarWarsData data) {
      Name = "Root";

      Field<CharacterInterface>("hero",
        resolve: context => data.GetDroidByIdAsync("3")
      );
      Field<HumanType>(
        "human",
        arguments: new QueryArguments(
            new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the human" }
        ),
        resolve: context => data.GetHumanByIdAsync(context.Argument<string>("id"))
      );
      Field<DroidType>(
        "droid",
        arguments: new QueryArguments(
            new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the droid" }
        ),
        resolve: context => data.GetDroidByIdAsync(context.Argument<string>("id"))
      );
    }
  }
}
