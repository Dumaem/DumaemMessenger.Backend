using FluentAssertions;

namespace Messenger.Tests;

public class ExperimentalTests
{
    [Fact]
    public void UnsuccessfulTest()
    {
        10.Should().Be(12);
    }
}