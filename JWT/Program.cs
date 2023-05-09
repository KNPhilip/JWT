global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using System.Text;
global using JWT.Entities;
global using JWT.Dtos;
global using JWT.Services;
global using JWT.Services.Interfaces;
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard authorization header using the bearer scheme: (\"bearer {Token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
});

var app = builder.Build();

ConfigureMapster();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

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

    config.ForType<(User baseUser, RefreshToken token), User>()
        .Map(dest => dest.RefreshToken, src => src.token.Token)
        .Map(dest => dest.TokenCreated, src => src.token.Created)
        .Map(dest => dest.TokenExpires, src => src.token.Expires)
        .Map(dest => dest, src => src.baseUser)
        .IgnoreNonMapped(true);
}