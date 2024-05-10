namespace JWT.Entities;

public sealed class User
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; } = DateTime.Now;
    public DateTime TokenExpires { get; set; }
}
