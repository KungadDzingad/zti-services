using Gateway.Helpers;
using Gateway.Helpers.Interfaces;
using Gateway.Middleware;
using Gateway.Models;
using Gateway.Services;
using Gateway.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<DbContext>();

// Services
builder.Services.AddScoped<IUserService, UserService>();

// Helpers
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
