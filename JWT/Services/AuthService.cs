namespace JWT.Services
{
    public class AuthService : IAuthService
    {
        private static IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ActionResult<string>> CreateTokenAsync(User user)
        {
            var jwt = "";

            await Task.Run(() =>
            {
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: creds
                );

                jwt = new JwtSecurityTokenHandler().WriteToken(token);
            });

            return jwt;
        }
    }
}