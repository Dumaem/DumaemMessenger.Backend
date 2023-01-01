using FluentAssertions;

namespace Messenger.Tests;

public class ExperimentalTests
{
    [Fact]
    public void ExperimentalTest_ShouldSuccess()
    {
        var result = 10;

        result.Should().Be(10);
    }
}