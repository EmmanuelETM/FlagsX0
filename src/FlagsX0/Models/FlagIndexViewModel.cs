using FlagsX0.Business.Services;
using FlagsX0.DTOs;

namespace FlagsX0.Models;

public class FlagIndexViewModel
{
    public Pagination<FlagDto>? Pagination { get; set; }
    public List<int> SelectOptions { get; set; } = [5, 10, 15];
    public string? Error { get; set; }
}