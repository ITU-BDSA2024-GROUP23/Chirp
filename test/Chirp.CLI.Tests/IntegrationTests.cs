using static Chirp.CLI.UserInterface;

namespace Chirp.CLI.Tests;

public class IntegrationTests
{

    [Fact]

    public void IntTestPrintCheepsInProperFormat()
    {
        //Arrange
        List<Cheep> cheeps = new()
        {
            new("axlu", "test0", 1690981487),
            new("vmem", "test1", 1690981487),
            new("kasjo", "test2", 1690981487)
        };

        using var sw = new StringWriter();
        Console.SetOut(sw);

        //Act
        PrintCheeps(cheeps);

        //Assert - Environment.Newline due to different OS
        var output = "axlu @ 02-08-2023 15:04:47: test0" + Environment.NewLine +
                    "vmem @ 02-08-2023 15:04:47: test1" + Environment.NewLine +
                    "kasjo @ 02-08-2023 15:04:47: test2" + Environment.NewLine;

        var result = sw.ToString();
        Assert.Equal(output, result);
    }


}