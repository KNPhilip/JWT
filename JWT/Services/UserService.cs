namespace JWT.Services;

public sealed class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GetMyName()
    {
        string result = string.Empty;
        if (_httpContextAccessor.HttpContext is not null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }

        return result;
    }
}
