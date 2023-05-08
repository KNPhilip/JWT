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

        [HttpGet("Message"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> GetName()
        {
            await Task.Run(() => {});
            return Ok("Hi.");
        }

        [HttpPost("Register"), AllowAnonymous]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            try
            {
                await Task.Run(() =>
                {
                    user = (user, request).Adapt<User>();
                });

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }

        [HttpPost("Login"), AllowAnonymous]
        public async Task<ActionResult<string>> Login(UserDto request)
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

                return Ok(await _authService.CreateTokenAsync(user));
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
    }
}