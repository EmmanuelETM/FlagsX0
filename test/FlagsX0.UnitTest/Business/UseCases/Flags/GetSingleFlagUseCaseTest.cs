using FlagsX0.Business.UseCases.Flags;
using FlagsX0.UnitTest.Services;
using Xunit.Abstractions;

namespace FlagsX0.UnitTest.Business.UseCases.Flags;

public class GetSingleFlagUseCaseTest(ITestOutputHelper testOutputHelper)
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    [Fact]
    public async Task WhenFlagExists_ThenReturnFlag()
    {
        // =========== Arrange ============
        _testOutputHelper.WriteLine("Starting test: WhenFlagExists_ThenReturnFlag\n");

        var userDetails = new FlagUserDetailsStub();
        _testOutputHelper.WriteLine($"Created user details with UserId: {userDetails.UserId}");

        // Use WithUserFlagsAsync to ensure the flag belongs to the current user
        var memoDb = await new MemoDb(userDetails)
            .WithUserFlagsAsync(userDetails.UserId, ("Flag1", true));
        var inMemoDb = memoDb.Context;

        _testOutputHelper.WriteLine($"Created database with Flags for user {userDetails.UserId}");


        // ============= Act ==============
        _testOutputHelper.WriteLine("\nExecuting GetSingleFlagUseCase for 'Flag1'");
        var useCase = new GetSingleFlagUseCase(inMemoDb);
        var result = await useCase.Execute("Flag1");

        // =========== Assert =============
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal("Flag1", result.Value.Name);
        Assert.True(result.Value.IsEnabled);

        _testOutputHelper.WriteLine($"\nResult - Success: {result.Success}");

        _testOutputHelper.WriteLine(result.Success
            ? $"Found flag - Name: '{result.Value.Name}', Value: {result.Value.IsEnabled}"
            : $"Error: {result.Errors.First().Message}");

        _testOutputHelper.WriteLine("\nTest completed successfully - Flag retrieved correctly");
    }

    [Fact]
    public async Task WhenFlagDoesNotExist_ThenReturnError()
    {
        // =========== Arrange ============
        _testOutputHelper.WriteLine("Starting test: WhenFlagDoesNotExist_ThenReturnError\n");

        var userDetails = new FlagUserDetailsStub();
        _testOutputHelper.WriteLine($"Created user details with UserId: {userDetails.UserId}");

        var memoDb = new MemoDb(userDetails);
        var inMemoDb = memoDb.Context;

        _testOutputHelper.WriteLine("Created empty database");


        // ============= Act ==============
        _testOutputHelper.WriteLine("\nExecuting GetSingleFlagUseCase for non-existent 'NonExistentFlag'");
        var useCase = new GetSingleFlagUseCase(inMemoDb);
        var result = await useCase.Execute("NonExistentFlag");

        // =========== Assert =============
        Assert.False(result.Success);
        Assert.Equal("Flag not found", result.Errors.First().Message);
        Assert.Null(result.Value);

        _testOutputHelper.WriteLine($"\nResult - Success: {!result.Success}");

        if (!result.Success)
        {
            _testOutputHelper.WriteLine($"Error message: {result.Errors.First().Message}");
            _testOutputHelper.WriteLine($"Total errors: {result.Errors.Length}");
        }

        _testOutputHelper.WriteLine("\nTest completed successfully - Non-existent flag correctly returned error");
    }
}