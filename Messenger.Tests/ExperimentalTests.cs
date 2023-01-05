using FluentAssertions;

namespace Messenger.Tests;

public class ExperimentalTests
{
    [Fact]
    public void Test_Success()
    {
        10.Should().Be(10);

        10.Should().Be(10);
    }

    [Fact]
    public void Test_Failure()
    {
        10.Should().Be(12);
    }
}