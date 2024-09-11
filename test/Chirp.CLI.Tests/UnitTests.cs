using static Chirp.CLI.UserInterface;

namespace Chirp.CLI.Tests;

public class UnitTests
{
    [Fact]

    public void TestUnixTimeStampToDateTime()
    {
        //Arrange
        string result = Cheep.UnixTimeStampToDateTime(1690981487);

        //Assert
        Assert.Equal("02-08-2023 15:04:47", result);
    }

    [Fact]
    public void TestToStringFormat()
    {
        //Arrange
        Cheep chirp = new("axlu", "test", 1690981487);

        //Assert
        Assert.Equal("axlu @ 02-08-2023 15:04:47: test", chirp.ToString());
    } 
}