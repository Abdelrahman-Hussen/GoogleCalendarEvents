using GoogleCalendarIntegration.Application.DI;
using GoogleCalendarIntegration.Infrastructure.DI;

using System.Text.Json.Serialization;
using GoogleCalendarIntegration.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.ApplicationStrapping();

builder.Services.InfrastructureStrapping();

builder.Services.AddAuthentication();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
sqlServerOptionsAction: options => {
    options.EnableRetryOnFailure();
    options.CommandTimeout(10);
}));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
