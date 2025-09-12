using FlagsX0.DTOs;

namespace FlagsX0.Models;

public class FlagIndexViewModel
{
    public required List<FlagDto> Flags { get; set; }
    public string? Error  { get; set; }
}