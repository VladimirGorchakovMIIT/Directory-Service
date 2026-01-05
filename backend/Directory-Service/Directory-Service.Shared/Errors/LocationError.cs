namespace Directory_Service.Shared.Errors;

public static class LocationError
{
    public static Error NameConflict(string? code, string? name)
    {
        var extensionCode = code is null ? "conflict.name" : code;
        return Error.Conflict(code, $"Сущность с таким названием уже существует {name}");
    }

    public static Error DatabaseError() =>
        Error.Failure("location.database.error", "Ошибка базы данных при работе с сервером - location");

    public static Error OperationCancelled() =>
        Error.Failure("operation.canceled.error", "Была произведена отмена взаимодействия с базой данных");
}