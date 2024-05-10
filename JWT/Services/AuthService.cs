namespace JWT.Services;

public sealed class AuthService : IAuthService
{
    private static IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(User user)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new(
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        string jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public RefreshToken CreateRefreshToken()
    {
        RefreshToken refreshToken = new()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
        };

        return refreshToken;
    }
}
