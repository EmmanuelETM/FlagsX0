using FlagsX0.Business.Services;

namespace FlagsX0.UnitTest.Services;

public class FlagUserDetailsStub(string userId) : IFlagUserDetails
{
    public FlagUserDetailsStub() : this(Guid.NewGuid().ToString())
    {
    }

    public string UserId { get; } = userId;
}