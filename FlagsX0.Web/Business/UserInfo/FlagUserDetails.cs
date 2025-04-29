using Microsoft.AspNetCore.Identity;

namespace FlagsX0.Web.Business.UserInfo
{
    public interface IFlagUserDetails
    {
        public string UserId { get; }
    }

    public class FlagUserDetails : IFlagUserDetails
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public FlagUserDetails(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string UserId
            => _userManager.GetUserId(_httpContextAccessor.HttpContext!.User) ?? throw new Exception("This workflow needs authentication!");
    }
}
