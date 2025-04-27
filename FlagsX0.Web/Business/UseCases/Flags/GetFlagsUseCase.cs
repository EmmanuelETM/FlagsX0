using FlagsX0.Web.Data;
using FlagsX0.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FlagsX0.Web.Business.UseCases.Flags
{
    public class GetFlagsUseCase
    {
        private readonly ApplicationDbContext _aplicationDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetFlagsUseCase(ApplicationDbContext aplicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _aplicationDbContext = aplicationDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<FlagDTO>> Execute()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await _aplicationDbContext.Flags.Where(x => x.UserId == userId).AsNoTracking().ToListAsync();

            return response.Select(a => new FlagDTO()
            {
                Name = a.Name,
                IsEnabled = a.Value
            }).ToList(); ;
        }
    }
}
