using Directory_Service.Application.DependencyInjection;
using Directory_Service.Core.DependencyInjection;
using Directory_Service.Core.Middlewares;
using Directory_Service.Infrastructure;
using Directory_Service.Infrastructure.DependencyInjection;using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApplicationDbContext>(_ => new ApplicationDbContext(builder.Configuration));
builder.Services.AddDirectoryService(builder.Configuration);

builder.Services.AddInfrastructureService();
builder.Services.AddApplicationService();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option => option.SwaggerEndpoint("/swagger/v1/swagger.json", "Directory Service"));
}

//Кастомные middlewares
app.UseException();

//Включаем логирование Http-запросов
app.UseSerilogRequestLogging();

app.MapControllers();
app.Run();