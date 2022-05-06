using Gateway.Helpers;
using Gateway.Helpers.Interfaces;
using Gateway.Middleware;
using Gateway.Models;
using Gateway.Services;
using Gateway.Services.Interfaces;
using Microsoft.AspNetCore.HttpOverrides;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDiscoveryClient();
builder.Services.AddHttpClient("message-service",client => client.BaseAddress = new Uri("http://message-service/")).AddServiceDiscovery();
builder.Services.AddDiscoveryClient();
builder.Services.AddHttpClient("posts-service",client => client.BaseAddress = new Uri("http://localhost:3000/")).AddServiceDiscovery();
builder.Services.AddDiscoveryClient();
builder.Services.AddHttpClient("logon-events",client => client.BaseAddress = new Uri("https://localhost:7194/")).AddServiceDiscovery();

// DbContext
builder.Services.AddDbContext<DbContext>();

// Services
builder.Services.AddScoped<IUserService, UserService>();

// Helpers
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseForwardedHeaders();

app.UseHttpsRedirection();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
