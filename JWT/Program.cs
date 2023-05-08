global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using System.Text;
global using JWT.Entities;
global using JWT.Dtos;
global using JWT.Services;
global using JWT.Services.Interfaces;
global using Mapster;
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

ConfigureMapster();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void ConfigureMapster()
{
    var config = TypeAdapterConfig.GlobalSettings;

    config.ForType<(User baseUser, UserDto dto), User>()
        .Map(dest => dest.Username, src => src.dto.Username)
        .Map(dest => dest.PasswordHash, src => BCrypt.Net.BCrypt.HashPassword(src.dto.Password))
        .Map(dest => dest, src => src.baseUser)
        .IgnoreNonMapped(true);
}