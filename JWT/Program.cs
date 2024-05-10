global using System.Text;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using JWT.Entities;
global using JWT.Dtos;
global using JWT.Services;
global using JWT.Services.Interfaces;
global using Swashbuckle.AspNetCore.Filters;
global using Mapster;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IUserService, UserService>();

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

builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

WebApplication app = builder.Build();

ConfigureMapster();

app.UseSwagger();

app.UseSwaggerUI();

app.UseCors("NgOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void ConfigureMapster()
{
    TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;

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
