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
    
    [Fact]
    public void ExperimentalTest_ShouldFail()
    {
        var result = 10;

        result.Should().Be(12);
    }
}