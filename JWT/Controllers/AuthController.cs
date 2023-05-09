namespace JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new();
        private static IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register"), AllowAnonymous]
        public ActionResult<User> Register(UserDto request)
        {
            try
            {
                user = (user, request).Adapt<User>();
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }

        [HttpPost("Login"), AllowAnonymous]
        public ActionResult<string> Login(UserDto request)
        {
            try
            {
                if (request.Username != user.Username)
                {
                    return BadRequest("User not found.");
                }
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return BadRequest("Incorrect password.");
                }

                var token = _authService.CreateToken(user);

                var refreshToken = _authService.CreateRefreshToken();
                SetRefreshToken(refreshToken);

                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }

        [HttpPost("Refresh-Token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh-Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("The Refresh-Token has expired.");
            }

            var token = _authService.CreateToken(user);
            var newRefreshToken = _authService.CreateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            user = (user, newRefreshToken).Adapt<User>();
        }
    }
}