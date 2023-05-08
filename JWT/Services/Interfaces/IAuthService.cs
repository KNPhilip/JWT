namespace JWT.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ActionResult<string>> CreateToken(User user);
    }
}