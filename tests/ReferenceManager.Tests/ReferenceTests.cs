namespace Reference.Tests;
using Xunit;

public class UnitTest1
{
    [Fact]
    // For the CI/CD test
    public void Test1()
    {
        int result = 2 + 2;
        Assert.Equal(4, result);
    }
}