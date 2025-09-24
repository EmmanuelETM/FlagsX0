using FlagsX0.Business.UseCases.Flags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;
using ROP.APIExtensions;

namespace FlagsX0.Controllers.Api;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FlagsController(GetSingleFlagUseCase getSingleFlag) : ControllerBase
{
    [ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status404NotFound)]
    [HttpGet("{flagName}")]
    public async Task<IActionResult> GetSingleFlag(string flagName)
    {
        var result = await getSingleFlag
            .Execute(flagName)
            .Map(a => a.IsEnabled)
            .ToActionResult();

        return result;
    }
}