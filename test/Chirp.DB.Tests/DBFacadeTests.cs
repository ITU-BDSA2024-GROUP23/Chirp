namespace Chirp.DB.Tests;

public class TestsFixture : IDisposable
{
    public DBFacade testDB;
    public readonly string dbCustomPath;
    private readonly string? _envBefore; // Dont know if necessary
    
    public TestsFixture()
    {
        dbCustomPath = Path.Combine(Path.GetTempPath(), "initializationTests.db");
        _envBefore = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        Environment.SetEnvironmentVariable("CHIRPDBPATH", null);
        File.Delete(DBFacade.DEFAULT_DB_PATH);
        File.Delete(dbCustomPath);

        testDB = new();
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("CHIRPDBPATH", _envBefore);
        File.Delete(DBFacade.DEFAULT_DB_PATH);
        File.Delete(dbCustomPath);
    }
}

public class DBFacadeTests : IClassFixture<TestsFixture>
{
    private readonly DBFacade _testDB;
    private readonly string _dbCustomPath;

    public DBFacadeTests(TestsFixture fixture)
    {
        _testDB = fixture.testDB;
        _dbCustomPath = fixture.dbCustomPath;
    }

    [Fact]
    public void DBCreatedInTempIf_CHIRPDBPATH_NotSet()
    {   
        long dbSize = new FileInfo(DBFacade.DEFAULT_DB_PATH).Length;

        Assert.Multiple(
            () => Assert.True(File.Exists(DBFacade.DEFAULT_DB_PATH)),
            () => Assert.True(dbSize > 0)
        );
    }

    [Fact]
    public void DBCreatedAt_CHIRPDBPATH_IfSet() 
    {
        Environment.SetEnvironmentVariable("CHIRPDBPATH", _dbCustomPath);
        DBFacade db = new();
        long dbSize = new FileInfo(_dbCustomPath).Length;

        Assert.Multiple(
            () => Assert.True(File.Exists(_dbCustomPath)),
            () => Assert.True(dbSize > 0)
        );
    }

    [Fact]
    public void GetCheepsReturnsCheeps()
    {
        List<CheepViewModel> cheeps = _testDB.GetCheeps(10, 0);
        Assert.True(cheeps.Any());
    }
}

