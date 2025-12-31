using CSharpFunctionalExtensions;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Department.ValueObjects;

public record Path(string Value)
{
    public static Result<Path, Error> Create(string identifier)
    {
        //TODO необходимо будет продумать и реализовать логику формирования пути
        return new Path(identifier);
    }
}