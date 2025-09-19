using FlagsX0.Data.Entities;
using FlagsX0.DTOs;

namespace FlagsX0.Business.Mappers;

public static class FlagEntityExtension
{
    public static FlagDto ToDto(this FlagEntity entity)
    {
        return new FlagDto(entity.Id, entity.Name, entity.Value);
    }

    public static List<FlagDto> ToDto(this List<FlagEntity> entities)
    {
        return entities.Select(ToDto).ToList();
    }
}