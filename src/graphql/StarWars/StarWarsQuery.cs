using GraphQL.StarWars.Types;
using GraphQL.Types;

namespace GraphQL.StarWars {
  public class StarWarsQuery : ObjectGraphType {
    // Test with localhost/graphql/?query={ droid { id name } }
    // Test with localhost/graphql/?query={ hero { id name } }
    public StarWarsQuery() {
    }

    public StarWarsQuery(StarWarsData data) {
      Name = "Query";

      Field<CharacterInterface>("hero",
        resolve: context => new Droid { Id = "1", Name = "R2-D2" }
        // resolve: context => data.GetDroidByIdAsync("3")
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
