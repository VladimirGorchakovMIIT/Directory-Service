using Directory_Service.Application.DependencyInjection;
using Directory_Service.Infrastructure;
using Directory_Service.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApplicationDbContext>(_ => new ApplicationDbContext(builder.Configuration));

builder.Services.AddInfrastructureService();
builder.Services.AddApplicationService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option => option.SwaggerEndpoint("/swagger/v1/swagger.json", "Directory Service"));
}

app.MapControllers();
app.Run();