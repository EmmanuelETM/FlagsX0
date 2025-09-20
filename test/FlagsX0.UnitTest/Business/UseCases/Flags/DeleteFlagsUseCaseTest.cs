using FlagsX0.Business.UseCases.Flags;
using FlagsX0.UnitTest.Services;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace FlagsX0.UnitTest.Business.UseCases.Flags;

public class DeleteFlagsUseCaseTest(ITestOutputHelper testOutputHelper)
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    [Fact]
    public async Task WhenFlagNotFound_ThenError()
    {
        // ================= Arrange =================
        _testOutputHelper.WriteLine("Starting test: WhenFlagNotFound_ThenError\n");

        var userDetails = new FlagUserDetailsStub();
        _testOutputHelper.WriteLine($"Created user details with UserId: {userDetails.UserId}");

        var inMemoDb = new MemoDb(userDetails).Context;
        _testOutputHelper.WriteLine("Created empty in-memory database");

        // ================= Act =================
        _testOutputHelper.WriteLine("\nAttempting to delete non-existing flag: 'NotExistingFlag'");
        var useCase = new DeleteFlagUseCase(inMemoDb);
        var result = await useCase.Execute("NotExistingFlag");

        // ================= Assert =================
        _testOutputHelper.WriteLine($"Result success: {!result.Success}");
        _testOutputHelper.WriteLine($"Error message: {(result.Success ? "None" : result.Errors.First().Message)}");

        Assert.False(result.Success);
        Assert.Equal("Flag not found", result.Errors.First().Message);

        _testOutputHelper.WriteLine("\nTest completed successfully");
    }

    [Fact]
    public async Task WhenFlagExists_ThenDelete()
    {
        // ================= Arrange =================
        _testOutputHelper.WriteLine("Starting test: WhenFlagExists_ThenDelete\n");

        var userDetails = new FlagUserDetailsStub();
        _testOutputHelper.WriteLine($"Created user details with UserId: {userDetails.UserId}");

        var memoDb = await new MemoDb(userDetails)
            .WithUserFlagsAsync(userDetails.UserId, ("TestFlag", true));
        var inMemoDb = memoDb.Context;
        _testOutputHelper.WriteLine("Created database with TestFlag=true\n");

        // ================= Act =================
        var initialCount = await inMemoDb.Flags.CountAsync();
        _testOutputHelper.WriteLine($"Initial flag count: {initialCount}");

        //Delete Flag
        _testOutputHelper.WriteLine("\nExecuting delete operation for 'TestFlag'");
        var useCase = new DeleteFlagUseCase(inMemoDb);
        var result = await useCase.Execute("TestFlag");

        // Verify flag is filtered out
        var activeCount = await inMemoDb.Flags.CountAsync();
        _testOutputHelper.WriteLine($"Active flag count after deletion: {activeCount}");

        // Verify flag is marked as deleted
        var deletedFlag = await memoDb.Context.Flags
            .IgnoreQueryFilters()
            .SingleAsync(f => f.Name == "TestFlag");
        _testOutputHelper.WriteLine(
            $"Retrieved deleted flag - IsDeleted: {deletedFlag.IsDeleted}, DeletedTime: {deletedFlag.DeletedTimeUtc}");

        // ================= Assert =================

        //Assert useCase executed Correctly
        _testOutputHelper.WriteLine($"\nDelete operation result - Success: {result.Success}, Value: {result.Value}");
        Assert.True(result.Success);
        Assert.True(result.Value);

        //Assert InitialCount and ActiveCount have correct values
        _testOutputHelper.WriteLine($"Verifying counts - Initial: {initialCount}, Active: {activeCount}");
        Assert.Equal(1, initialCount);
        Assert.Equal(0, activeCount);

        //Assert flag got deleted correctly
        _testOutputHelper.WriteLine(
            $"Verifying soft delete - IsDeleted: {deletedFlag.IsDeleted}, UserId match: {deletedFlag.UserId == userDetails.UserId}");
        Assert.True(deletedFlag.IsDeleted);
        Assert.NotNull(deletedFlag.DeletedTimeUtc);
        Assert.True(deletedFlag.DeletedTimeUtc <= DateTime.UtcNow);
        Assert.Equal(userDetails.UserId, deletedFlag.UserId);

        _testOutputHelper.WriteLine("\nTest completed successfully");
    }
}