namespace JWT.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ClaimController(IClaimService claimService) : ControllerBase
{
    private readonly IClaimService _claimService = claimService;

    [HttpGet("NameFromClaims"), Authorize(Roles = "Admin")]
    public ActionResult<object> NameFromClaims()
    {
        try
        {
            string name = _claimService.GetNameFromClaims();
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
            List<string> claims = _claimService.GetRolesFromClaims();
            return Ok(new { claims });
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }
}
