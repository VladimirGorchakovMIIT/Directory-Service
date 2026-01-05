using CSharpFunctionalExtensions;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Domain.Department.ValueObjects;

public record Depth(int Value)
{
    public static Result<Depth, Error> Create(int depth)
    {
        //TODO необходимо будет произвести подсчет глубину Department
        return new Depth(depth);
    }
}