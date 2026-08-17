using BackendChallange.Api.Interfaces;
using BackendChallange.Api.Models;
using BackendChallange.Api.Repositories;
using BackendChallange.Api.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<
    IPasswordHasher<User>,
    PasswordHasher<User>
>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Backend Challenge API funcionando!");

app.MapControllers();

app.Run();