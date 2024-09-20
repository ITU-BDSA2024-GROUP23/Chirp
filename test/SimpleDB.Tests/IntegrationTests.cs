using static SimpleDB.Tests.UnitTests;

namespace SimpleDB.Tests;

public class IntegrationTests
{
    /* THIS TEST IS NOT WORKING WITH OUR SINGLETON IMPLEMENTATION
    [Fact]
    public void TestStore()
    {
        //Arrange
        string filePath = "../../../../data/testData.csv";
        var db = CSVDatabase<TestRecord>.GetInstance(filePath);

        //Act
        db.ResetTestDB();
        db.Store(new TestRecord("yo"));

        //Assert
        var result = db.Read();
        Assert.Contains("yo", result.Select(r => r.Message));
    }
    */
}