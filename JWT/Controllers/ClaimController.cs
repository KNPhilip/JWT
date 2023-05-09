using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpGet("NameFromClaims"), Authorize(Roles = "Admin")]
        public ActionResult<object> NameFromClaims()
        {
            try
            {
                var name = _claimService.GetNameFromClaims();
                return Ok(new { name });
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }

        [HttpGet("RolesFromClaims"), Authorize(Roles = "Admin")]
        public ActionResult<object> RolesFromClaims()
        {
            try
            {
                var claims = _claimService.GetRolesFromClaims();
                return Ok(new { claims });
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
    }
}