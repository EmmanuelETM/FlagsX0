using FlagsX0.Data.Entities;
using FlagsX0.DTOs;

namespace FlagsX0.Business.Mappers;

public static class FlagEntityExtension
{
    public static FlagDto ToDto(this FlagEntity entity)
        => new()
        {
            Name = entity.Name,
            IsEnabled = entity.Value
        };
    
    public static List<FlagDto> ToDto(this List<FlagEntity> entities) 
    => entities.Select(ToDto).ToList();
}