using FlagsX0.Business.UseCases.Flags;
using FlagsX0.DTOs;
using FlagsX0.UnitTest.Services;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace FlagsX0.UnitTest.Business.UseCases.Flags;

public class UpdateFlagUseCaseTest(ITestOutputHelper testOutputHelper)
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    [Fact]
    public async Task WhenFlagExists_ThenUpdate()
    {
        // =========== Arrange ============
        _testOutputHelper.WriteLine("Starting test: WhenFlagExists_ThenUpdateSuccessfully\n");

        var userDetails = new FlagUserDetailsStub();

        var memoDb = await new MemoDb(userDetails)
            .WithUserFlagsAsync(userDetails.UserId, ("OriginalFlag", false));
        var inMemoDb = memoDb.Context;

        // Get the flag ID from the db
        var existingFlag = await inMemoDb.Flags.SingleAsync(f => f.Name == "OriginalFlag");

        // FlagDto to change both the Name and IsEnabled
        var updateDto = new FlagDto(existingFlag.Id, "UpdatedFlag", true);

        _testOutputHelper.WriteLine("Created memory database\n");
        _testOutputHelper.WriteLine(
            $"Created flag - ID: {existingFlag.Id}, Name: '{existingFlag.Name}', Value: {existingFlag.Value}");
        _testOutputHelper.WriteLine(
            $"Update DTO - ID: {updateDto.Id}, Name: '{updateDto.Name}', IsEnabled: {updateDto.IsEnabled}");

        // ============= Act ==============
        _testOutputHelper.WriteLine("\nExecuting UpdateFlagUseCase");
        var useCase = new UpdateFlagUseCase(inMemoDb);
        var result = await useCase.Execute(updateDto);

        // =========== Assert =============
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(updateDto.Id, result.Value.Id);
        Assert.Equal("UpdatedFlag", result.Value.Name);
        Assert.True(result.Value.IsEnabled);

        _testOutputHelper.WriteLine($"\nUpdate result - Success: {result.Success}");
        _testOutputHelper.WriteLine(
            result.Success
                ? $"Updated flag - ID: {result.Value.Id}, Name: '{result.Value.Name}', IsEnabled: {result.Value.IsEnabled}"
                : $"Error: {result.Errors.First().Message}");

        // Verify in database
        var updatedFlag = await inMemoDb.Flags.SingleAsync(f => f.Id == existingFlag.Id);
        _testOutputHelper.WriteLine($"Database verification - Name: '{updatedFlag.Name}', Value: {updatedFlag.Value}");

        Assert.Equal("UpdatedFlag", updatedFlag.Name);
        Assert.True(updatedFlag.Value);

        if (result.Success) _testOutputHelper.WriteLine("\nTest completed successfully - Flag updated correctly");
    }

    [Fact]
    public async Task WhenUpdatingOnlyValue_ThenSucceed()
    {
        // =========== Arrange ============
        _testOutputHelper.WriteLine("Starting test: WhenUpdatingOnlyValue_ThenSucceed\n");

        var userDetails = new FlagUserDetailsStub();
        var memoDb = await new MemoDb(userDetails)
            .WithUserFlagsAsync(userDetails.UserId, ("ToggleFlag", false));
        var inMemoDb = memoDb.Context;

        var existingFlag = await inMemoDb.Flags.SingleAsync(f => f.Name == "ToggleFlag");

        // Only change the value, keep same name
        var toggleDto = new FlagDto(existingFlag.Id, "ToggleFlag", true);

        _testOutputHelper.WriteLine("Created database with ToggleFlag=false");
        _testOutputHelper.WriteLine($"Original flag - ID: {existingFlag.Id}, Value: {existingFlag.Value}");
        _testOutputHelper.WriteLine("Toggling flag value from false to true");

        // ============= Act ==============
        _testOutputHelper.WriteLine("Executing UpdateFlagUseCase to toggle value");
        var useCase = new UpdateFlagUseCase(inMemoDb);
        var result = await useCase.Execute(toggleDto);

        // =========== Assert =============
        _testOutputHelper.WriteLine($"Toggle result - Success: {result.Success}");

        if (result.Success) _testOutputHelper.WriteLine($"Toggled flag - IsEnabled: {result.Value.IsEnabled}");

        var toggledFlag = await inMemoDb.Flags.SingleAsync(f => f.Id == existingFlag.Id);

        Assert.True(result.Success);
        Assert.True(result.Value.IsEnabled);

        // Verify database reflects the change
        Assert.True(toggledFlag.Value);
        Assert.Equal("ToggleFlag", toggledFlag.Name); // Name unchanged

        _testOutputHelper.WriteLine("Test completed successfully - Value toggle worked");
    }

    [Fact]
    public async Task WhenFlagNameConflictsWithAnotherFlag_ThenError()
    {
        // ========== Arrange ============
        _testOutputHelper.WriteLine("Starting test: WhenFlagDoesNotExist_ThenError\n");
        var userDetails = new FlagUserDetailsStub();
        var memoDb = await new MemoDb(userDetails).WithFlagsAsync();
        var inMemoDb = memoDb.Context;

        // FlagDto with conflicting name
        var updateDto = new FlagDto(3, "Flag1", true);
        _testOutputHelper.WriteLine("Created database with Data");
        _testOutputHelper.WriteLine("Created FlagDto with Conflicting Name");

        // ============= Act =============
        var useCase = new UpdateFlagUseCase(inMemoDb);
        var result = await useCase.Execute(updateDto);

        _testOutputHelper.WriteLine("\nExecuting UpdateFlagUseCase\n");
        // ============= Assert ==========

        Assert.False(result.Success);
        Assert.Equal("Flag with that name already exists", result.Errors.First().Message);
        Assert.Null(result.Value);

        if (!result.Success) _testOutputHelper.WriteLine("Test completed successfully - Value toggle worked");
    }
}