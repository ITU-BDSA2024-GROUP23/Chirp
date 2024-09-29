namespace Chirp.DB.Tests;

public abstract class TestsBase : IDisposable
{
    protected readonly string dbDefaultPath = Path.Combine(Path.GetTempPath(), "chirp.db");
    protected readonly string dbCustomPath = Path.Combine(Path.GetTempPath(), "chirp2.db");

    protected TestsBase()
    {
        Environment.SetEnvironmentVariable("CHIRPDBPATH", null);
        File.Delete(dbDefaultPath);
        File.Delete(dbCustomPath);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("CHIRPDBPATH", null);
        File.Delete(dbDefaultPath);
        File.Delete(dbCustomPath);
    }
}

public class DBFacadeInitializationTests : TestsBase
{
    [Fact]
    public void DBCreatedInTempIfEnvironmentVariableNotSet()
    {   
        DBFacade db = new();

        Assert.Multiple(
            () => Assert.True(File.Exists(dbDefaultPath)),
            () => Assert.True(new FileInfo(dbDefaultPath).Length > 0)
        );
    }

    [Fact]
    public void DBCreatedAtEnvironmentVariableIfSet() 
    {
        Environment.SetEnvironmentVariable("CHIRPDBPATH", dbCustomPath);
        DBFacade db = new();

        Assert.Multiple(
            () => Assert.True(File.Exists(dbCustomPath)),
            () => Assert.True(new FileInfo(dbCustomPath).Length > 0)
        );
    }
}