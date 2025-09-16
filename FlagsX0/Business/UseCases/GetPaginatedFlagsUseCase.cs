using FlagsX0.Business.Mappers;
using FlagsX0.Business.Services;
using FlagsX0.Data;
using FlagsX0.Data.Entities;
using FlagsX0.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Business.UseCases;

public class GetPaginatedFlagsUseCase(ApplicationDbContext dbContext, IFlagUserDetails userDetails)
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IFlagUserDetails _userDetails = userDetails;

    public async Task<Result<Pagination<FlagDto>>> Execute(string? search, int page, int size)
    {
        return await ValidatePage(page).Fallback(_ =>
            {
                page = 1;
                return Result.Unit;
            })
            .Bind(_ => ValidateSize(size).Fallback(_ =>
            {
                size = 5;
                return Result.Unit;
            }))
            .Async()
            .Bind(x => GetFromDb(search, page, size))
            .Map(x => x.ToDto())
            .Combine(x => TotalElements(search))
            .Map(x => new Pagination<FlagDto>(x.Item1, x.Item2, size, page, search));
    }

    private Result<Unit> ValidatePage(int page)
    {
        return page < 1 ? Result.Failure("Page not supported") : Result.Unit;
    }

    private Result<Unit> ValidateSize(int size)
    {
        int[] allowedValues = [5, 10, 15];

        return !allowedValues.Contains(size) ? Result.Failure("Page size not supported") : Result.Unit;
    }

    private async Task<Result<List<FlagEntity>>> GetFromDb(string? search, int page, int size)
    {
        var query = _dbContext.Flags
            .Where(x => x.UserId == _userDetails.UserId)
            .Skip(size * (page - 1))
            .Take(size);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase));

        return await query.ToListAsync();
    }

    private async Task<Result<int>> TotalElements(string? search)
    {
        var query = _dbContext.Flags
            .Where(x => x.UserId == _userDetails.UserId);

        if (!string.IsNullOrWhiteSpace(search))
            // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall
            query = query.Where(x => x.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase));

        return await query.CountAsync();
    }
}