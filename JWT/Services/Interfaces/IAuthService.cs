namespace JWT.Services.Interfaces;

public interface IAuthService
{
    string CreateToken(User user);
    RefreshToken CreateRefreshToken();
}
