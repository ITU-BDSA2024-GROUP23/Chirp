namespace Chirp.DB.Tests;
/*
These tests doesn't work when running other tests with shared resources.
TODO: I don't know how to fix it yet, but maybe one of you can figure it out.
*/

/*
public abstract class InitializationTestsBase : IDisposable
{
    protected readonly string dbCustomPath = Path.Combine(Path.GetTempPath(), "chirpInit.db");

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

[Collection("Sequential")]
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
    public DBFacade testDB;
    
    public QueryTestsFixture()
    {
        testDB = new();
    }

    public void Dispose()
    {
        File.Delete(DBFacade.DEFAULT_DB_PATH);
    }
}

[Collection("Sequential")]
public class DBFacadeQueryTests : IClassFixture<QueryTestsFixture>
{
    DBFacade testDB;

    public DBFacadeQueryTests(QueryTestsFixture fixture)
    {
        testDB = fixture.testDB;
    }

    [Fact]
    public void GetCheepsReturnsCheeps()
    {
        List<CheepViewModel> cheeps = testDB.GetCheeps(10, 0);
        Assert.True(cheeps.Count != 0);
    }
}
*/
