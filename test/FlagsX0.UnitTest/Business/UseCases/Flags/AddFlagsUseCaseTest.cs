using FlagsX0.Business.UseCases.Flags;
using FlagsX0.Data.Entities;
using FlagsX0.UnitTest.Services;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace FlagsX0.UnitTest.Business.UseCases.Flags;

public class AddFlagsUseCaseTest(ITestOutputHelper testOutputHelper)
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    [Fact]
    public async Task WhenFlagNameAlreadyExists_ThenError()
    {
        // ================= Arrange =================
        _testOutputHelper.WriteLine("Starting test: WhenFlagNameAlreadyExists_ThenError\n");

        var userDetails = new FlagUserDetailsStub();
        _testOutputHelper.WriteLine($"Created user details with UserId: {userDetails.UserId}");

        var inMemoDb = new MemoDb(userDetails).Context;
        _testOutputHelper.WriteLine("Created empty in-memory database");

        var currentFlag = new FlagEntity
        {
            UserId = userDetails.UserId,
            Name = "TestFlag",
            Value = true
        };
        _testOutputHelper.WriteLine(
            $"Creating existing flag - Name: '{currentFlag.Name}', Value: {currentFlag.Value}, UserId: {currentFlag.UserId}\n");

        inMemoDb.Flags.Add(currentFlag);
        await inMemoDb.SaveChangesAsync();

        var existingFlagsCount = await inMemoDb.Flags.CountAsync();
        _testOutputHelper.WriteLine($"Database seeded successfully. Existing flags count: {existingFlagsCount}\n");

        // ================= Act =================
        _testOutputHelper.WriteLine(
            $"Attempting to add duplicate flag with name: '{currentFlag.Name}' and value: true");

        var addFlagUseCase = new AddFlagUseCase(inMemoDb, userDetails);
        var result = await addFlagUseCase.Execute(currentFlag.Name, true);

        // ================= Assert =================
        _testOutputHelper.WriteLine($"Add operation result - Success: {!result.Success}");

        if (!result.Success)
        {
            _testOutputHelper.WriteLine($"Error message: {result.Errors.First().Message}");
            _testOutputHelper.WriteLine($"Total errors: {result.Errors.Length}");
        }

        var finalFlagsCount = await inMemoDb.Flags.CountAsync();
        _testOutputHelper.WriteLine($"Final flags count: {finalFlagsCount} (should remain {existingFlagsCount})\n");

        Assert.False(result.Success);
        Assert.Equal("This flag already exists", result.Errors.First().Message);

        _testOutputHelper.WriteLine("Test completed successfully - Duplicate flag was correctly rejected");
    }

    [Fact]
    public async Task WhenFlagDoesNotExist_ThenInsertOnDb()
    {
        // ================= Arrange =================
        _testOutputHelper.WriteLine("Starting test: WhenFlagDoesNotExist_ThenInsertOnDb\n");

        var userDetails = new FlagUserDetailsStub();
        _testOutputHelper.WriteLine($"Created user details with UserId: {userDetails.UserId}");

        var inMemoDb = new MemoDb(userDetails).Context;
        _testOutputHelper.WriteLine("Created empty in-memory database");

        var initialFlagsCount = await inMemoDb.Flags.CountAsync();
        _testOutputHelper.WriteLine($"Initial flags count: {initialFlagsCount}\n");

        // ================= Act =================
        const string flagName = "flagName";
        const bool flagValue = true;
        _testOutputHelper.WriteLine($"Attempting to add new flag - Name: '{flagName}', Value: {flagValue}");

        var addFlagUseCase = new AddFlagUseCase(inMemoDb, userDetails);
        var result = await addFlagUseCase.Execute(flagName, flagValue);

        // ================= Assert =================
        _testOutputHelper.WriteLine($"\nAdd operation result - Success: {result.Success}, Value: {result.Value}");

        var finalFlagsCount = await inMemoDb.Flags.CountAsync();
        _testOutputHelper.WriteLine($"\nFinal flags count: {finalFlagsCount} (expected: {initialFlagsCount + 1})");

        // Verify the flag was actually added with correct values
        var addedFlag = await inMemoDb.Flags
            .Where(f => f.Name == flagName)
            .SingleOrDefaultAsync();

        if (addedFlag != null)
        {
            _testOutputHelper.WriteLine(
                $"Added flag details - Name: '{addedFlag.Name}', Value: {addedFlag.Value}, UserId: {addedFlag.UserId}");
            _testOutputHelper.WriteLine($"Flag ID: {addedFlag.Id}");
        }
        else
        {
            _testOutputHelper.WriteLine("\nWARNING: Added flag not found in database!");
        }

        Assert.True(result.Success);
        Assert.True(result.Value);

        // Additional assertions to verify the flag was properly stored
        Assert.NotNull(addedFlag);
        Assert.Equal(flagName, addedFlag.Name);
        Assert.Equal(flagValue, addedFlag.Value);
        Assert.Equal(userDetails.UserId, addedFlag.UserId);
        Assert.Equal(initialFlagsCount + 1, finalFlagsCount);

        _testOutputHelper.WriteLine("\nTest completed successfully - New flag was added correctly");
    }
}