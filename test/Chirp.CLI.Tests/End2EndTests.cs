using System.Diagnostics;

namespace Chirp.CLI.Tests;

public class End2EndTests
{
    [Fact]
    public void TestReadCheep()
    {
        //Arrange
        var filePath = Directory.GetCurrentDirectory();
        var chirpCLIPath = Path.GetFullPath(Path.Combine(filePath, "..", "..", "..", "..", "..", "src", "Chirp.CLI", "bin", "Debug", "net7.0", "Chirp.CLI"));
        
        //Act
        var process = new Process {
            StartInfo = new ProcessStartInfo {
                //FileName = "dotnet",
                //Arguments = $"{chirpCLIPath} read",
                FileName = "pwd",
                RedirectStandardOutput = true,
                UseShellExecute = false,
            }
        };
        process.Start();
        StreamReader sr = process.StandardOutput;
        string output = sr.ReadToEnd();
        process.WaitForExit();

        string[] cheeps = output.Split("\n");


        //Assert
        Assert.Equal("new string[10]", output);
        //Assert.NotEmpty(cheeps);
    }

    [Fact]
    public void TestReadCheep2()
    {
        //Arrange
        var filePath = Directory.GetCurrentDirectory();
        var chirpCLIPath = Path.GetFullPath(Path.Combine(filePath, "..", "..", "..", "..", "..", "src", "Chirp.CLI", "bin", "Debug", "net7.0", "Chirp.CLI"));
        
        //Act
        var process = new Process {
            StartInfo = new ProcessStartInfo {
                FileName = "dotnet",
                Arguments = $"{chirpCLIPath} read",
                RedirectStandardOutput = true,
                UseShellExecute = false,
            }
        };
        process.Start();
        StreamReader sr = process.StandardOutput;
        string output = sr.ReadToEnd();
        process.WaitForExit();

        string[] cheeps = output.Split("\n");

        //Assert
        Assert.NotEmpty(cheeps);
        Assert.Equal("ropf", cheeps[0]);
    }






    [Fact]
    public void TestReadCheep3()
    {
        //Arrange
        //var filePath = Directory.GetCurrentDirectory();
        //var chirpCLIPath = Path.GetFullPath(Path.Combine(filePath, "..", "..", "..", "..", "..", "src", "Chirp.CLI", "bin", "Debug", "net7.0", "Chirp.CLI"));
        
        //Act
        var process = new Process {
            StartInfo = new ProcessStartInfo {
                FileName = "../../../../../src/Chirp.CLI/bin/Debug/net7.0/Chirp.CLI",
                Arguments = "read",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            }
        };
        
        process.Start();
        StreamReader sr = process.StandardOutput;
        string output = sr.ReadToEnd();
        process.WaitForExit();

        string[] cheeps = output.Split("\n");


        //Assert
        Assert.Equal("new string[10]", output);
        //Assert.NotEmpty(cheeps);
    }
}