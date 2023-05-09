namespace JWT.Services.Interfaces
{
    public interface IClaimService
    {
        public string GetNameFromClaims();

        public object GetRolesFromClaims();
    }
}