namespace Chirp.DB.Tests;

public abstract class InitializationTestsBase : IDisposable
{
    protected readonly string dbCustomPath = Path.Combine(Path.GetTempPath(), "chirp2.db");

    protected InitializationTestsBase()
    {
        Environment.SetEnvironmentVariable("CHIRPDBPATH", null);
        File.Delete(DBFacade.DEFAULT_DB_PATH);
        File.Delete(dbCustomPath);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("CHIRPDBPATH", null);
        File.Delete(DBFacade.DEFAULT_DB_PATH);
        File.Delete(dbCustomPath);
    }
}

public class DBFacadeInitializationTests : InitializationTestsBase
{
    [Fact]
    public void DBCreatedInTempIfEnvironmentVariableNotSet()
    {   
        DBFacade db = new();
        long dbSize = new FileInfo(DBFacade.DEFAULT_DB_PATH).Length;

        Assert.Multiple(
            () => Assert.True(File.Exists(DBFacade.DEFAULT_DB_PATH)),
            () => Assert.True(dbSize > 0)
        );
    }

    [Fact]
    public void DBCreatedAtEnvironmentVariableIfSet() 
    {
        Environment.SetEnvironmentVariable("CHIRPDBPATH", dbCustomPath);
        DBFacade db = new();
        long dbSize = new FileInfo(dbCustomPath).Length;

        Assert.Multiple(
            () => Assert.True(File.Exists(dbCustomPath)),
            () => Assert.True(dbSize > 0)
        );
    }
}

public class QueryTestsFixture : IDisposable
{
    readonly DBFacade testDB;
    public QueryTestsFixture ()
    {
        testDB = new();
    }

    public void Dispose()
    {
        File.Delete(DBFacade.DEFAULT_DB_PATH);
    }
}

public class DBFacadeQueryTests : IClassFixture<QueryTestsFixture>
{
    
}