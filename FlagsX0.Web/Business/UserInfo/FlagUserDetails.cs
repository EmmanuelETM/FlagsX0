using Microsoft.AspNetCore.Identity;

namespace FlagsX0.Web.Business.UserInfo
{
    public interface IFlagUserDetails
    {
        public string UserId { get; }
    }

    public class FlagUserDetails(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager) : IFlagUserDetails
    {
        public string UserId
            => userManager.GetUserId(httpContextAccessor.HttpContext!.User) ?? throw new Exception("This workflow needs authentication!");
    }
}
