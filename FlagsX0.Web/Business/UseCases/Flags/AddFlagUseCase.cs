using FlagsX0.Web.Data;
using FlagsX0.Web.Data.Entities;
using System.Security.Claims;


namespace FlagsX0.Web.Business.UseCases.Flags
{
    public class AddFlagUseCase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHttpContextAccessor _httpContextAccesor;

        public AddFlagUseCase(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccesor)
        {
            _applicationDbContext = applicationDbContext;
            _httpContextAccesor = httpContextAccesor;
        }

        public async Task<bool> Execute(string flagName, bool IsEnabled)
        {
            string userId = _httpContextAccesor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            FlagEntity entity = new()
            {
                Name = flagName,
                UserId = userId,
                Value = IsEnabled,
            };

            var response = await _applicationDbContext.Flags.AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }
    }
}
