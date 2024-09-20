using System.Diagnostics;

namespace Chirp.CLI.Tests;

public class End2EndTests
{
    /*
    [Fact]
    public void TestReadCheep()
    {
        //Arrange
        string dir = "src/Chirp.CLI/bin/Debug/net7.0/Chirp.CLI.dll";
        string args = "read";
        string output;

        //Act
        var process = new Process
        {
            StartInfo = {
                FileName = "dotnet",
                Arguments = "exec " + dir + " " + args,
                UseShellExecute = false,
                WorkingDirectory = Path.Combine("..", "..", "..", "..", ".."),
                RedirectStandardOutput = true
            }
        };
        process.Start();
        output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        string[] cheeps = output.Split("\n");
        string firstCheep = cheeps[0];

        //Assert
        Assert.StartsWith("ropf", firstCheep);
        Assert.NotEmpty(cheeps);
        Assert.EndsWith("Hello, BDSA students!", firstCheep);
    }

    [Fact]
    public void TestWriteCheep()
    {
        //Arrange
        string dir = "src/Chirp.CLI/bin/Debug/net7.0/Chirp.CLI.dll";
        string args = "cheep end2end test";
        string output;

        //Act
        var process = new Process
        {
            StartInfo = {
                FileName = "dotnet",
                Arguments = "exec " + dir + " " + args,
                UseShellExecute = false,
                WorkingDirectory = Path.Combine("..", "..", "..", "..", ".."),
                RedirectStandardOutput = true
            }
        };
        process.Start();
        output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        //Assert
        Assert.Equal("Cheeped: end2end test\n", output);
    }
    */
}
