namespace JWT.Services;

public sealed class ClaimService(IHttpContextAccessor httpContextAccessor) : IClaimService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GetNameFromClaims()
    {
        string result = string.Empty;
        if (_httpContextAccessor.HttpContext is not null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }

        return result;
    }

    public List<string> GetRolesFromClaims()
    {
        List<string> result = [];
        if (_httpContextAccessor.HttpContext is not null)
        {
            result = _httpContextAccessor.HttpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
        }

        return result;
    }
}
