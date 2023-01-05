using FluentAssertions;

namespace Messenger.Tests;

public class ExperimentalTests
{
    //Dumayu
    [Fact]
    public void Test_Success()
    {
        10.Should().Be(10);
    }
}