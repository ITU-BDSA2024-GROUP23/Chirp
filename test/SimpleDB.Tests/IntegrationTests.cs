using static SimpleDB.Tests.UnitTests;

namespace SimpleDB.Tests;

public class IntegrationTests
{
    [Fact]
    public void TestStore()
    {
        //Arrange
        string filePath = "../../../../data/testData.csv";
        var db = new CSVDatabase<TestRecord>(filePath);

        //Act
        db.ResetTestDB();
        db.Store(new TestRecord("yo"));

        //Assert
        var result = db.Read();
        Assert.Contains("yo", result.Select(r => r.Message));
    }
}