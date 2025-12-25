using CSharpFunctionalExtensions;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Department;

public record Depth(string Value)
{
    public static Result<Depth, Error> Create(string depth)
    {
        //TODO необходимо будет произвести подсчет глубину Department
        return new Depth(depth);
    }
}