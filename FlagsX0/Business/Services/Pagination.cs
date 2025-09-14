namespace FlagsX0.Business.Services;

public record Pagination<T>(List<T> Items, int TotalItems, int PageSize, int CurrentPage, string? Search);