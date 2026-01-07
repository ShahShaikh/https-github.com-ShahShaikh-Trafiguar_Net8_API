using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Trafigura_Assessment_API.Startup;
using TrafiguraAssessment.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Trade Transactions",
        Version = "V1",
        Description = "Trafigura Asessement- Trade Txn Service API",
    });
});
builder.Services.AddApplicationServices();

var connString = builder.Configuration.GetConnectionString("Assessment_DB");
builder.Services.AddDbContext<AssessmentDBContext>(options =>
    options.UseSqlServer(connString));


// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();
// Use CORS
app.UseCors("AllowAngularApp");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trade Transaction V1");
    c.RoutePrefix = string.Empty;  // Serve Swagger UI at the root
});


app.MapControllers();
app.Run();