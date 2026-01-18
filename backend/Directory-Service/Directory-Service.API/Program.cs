using Directory_Service.Core.DependencyInjection;
using Directory_Service.Core.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option => option.SwaggerEndpoint("/swagger/v1/swagger.json", "Directory Service"));
}

//Кастомные middleware
app.UseException();

//Включаем логирование Http-запросов
app.UseSerilogRequestLogging();

app.MapControllers();
app.Run();

namespace DirectoryService.API
{
    public partial class Program;
}