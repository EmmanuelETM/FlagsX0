using FlagsX0.Web.Data.Entities;
using FlagsX0.Web.DTOs;

namespace FlagX0.Web.Business.Mappers
{
    public static class FlagEntityExtension
    {
        public static FlagDTO ToDto(this FlagEntity entity)
            => new FlagDTO(entity.Name, entity.Value, entity.Id);

        public static List<FlagDTO> ToDTO(this List<FlagEntity> entities)
            => entities.Select(ToDto).ToList();
    }
}