namespace JWT.Services.Interfaces;

public interface IClaimService
{
    public string GetNameFromClaims();
    public List<string> GetRolesFromClaims();
}
