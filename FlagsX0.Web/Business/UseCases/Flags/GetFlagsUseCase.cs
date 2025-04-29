using FlagsX0.Web.Business.UserInfo;
using FlagsX0.Web.Data;
using FlagsX0.Web.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FlagsX0.Web.Business.UseCases.Flags
{
    public class GetFlagsUseCase
    {
        private readonly ApplicationDbContext _aplicationDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFlagUserDetails _userDetails;

        public GetFlagsUseCase(ApplicationDbContext aplicationDbContext, IHttpContextAccessor httpContextAccessor, IFlagUserDetails flagUserDetails)
        {
            _aplicationDbContext = aplicationDbContext;
            _httpContextAccessor = httpContextAccessor;
            _userDetails = flagUserDetails;
        }

        public async Task<List<FlagDTO>> Execute()
        {

            var response = await _aplicationDbContext
                .Flags
                .Where(x => x.UserId == _userDetails.UserId)
                .AsNoTracking()
                .ToListAsync();

            return response.Select(a => new FlagDTO()
            {
                Name = a.Name,
                IsEnabled = a.Value
            }).ToList(); ;
        }
    }
}
