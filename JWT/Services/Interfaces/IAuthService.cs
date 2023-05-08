namespace JWT.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ActionResult<string>> CreateTokenAsync(User user);
    }
}