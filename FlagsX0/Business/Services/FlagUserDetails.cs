using Microsoft.AspNetCore.Identity;

namespace FlagsX0.Business.Services;

public interface IFlagUserDetails
{
    public string UserId { get; }
}

public class FlagUserDetails(IHttpContextAccessor httpContext, UserManager<IdentityUser> userManager) : IFlagUserDetails
{
    private readonly IHttpContextAccessor _httpContext = httpContext;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    
    public string UserId => _userManager.GetUserId(_httpContext.HttpContext!.User) 
                            ?? throw new Exception("This workflow requires authentication");
}