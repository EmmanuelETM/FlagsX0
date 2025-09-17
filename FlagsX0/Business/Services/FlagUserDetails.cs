using System.Security.Claims;

namespace FlagsX0.Business.Services;

public interface IFlagUserDetails
{
    public string UserId { get; }
}

public class FlagUserDetails(IHttpContextAccessor httpContext) : IFlagUserDetails
{
    private readonly IHttpContextAccessor _httpContext = httpContext;

    public string UserId => _httpContext
                                .HttpContext?
                                .User
                                .FindFirstValue(ClaimTypes.NameIdentifier)!
                            ?? throw new Exception("This workflow require authentication");
}