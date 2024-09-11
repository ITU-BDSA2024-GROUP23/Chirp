namespace SimpleDB.Tests;

public class UnitTests
{
    [Fact]
    public void TestFileNotFound() 
    {
        //Arrange
        string path = "notfound.csv";
        Console.WriteLine(Environment.CurrentDirectory);
        
        //Assert
        Assert.Throws<FileNotFoundException>(() => new CSVDatabase<TestRecord>(path));
    }

    [Fact]
    public void TestRead()
    {
        //Arrange
        string filePath ="../../../../data/testData.csv";
        var db = new CSVDatabase<TestRecord>(filePath);

        //Act
        var result = db.Read();

        //Assert
        Assert.IsType<List<TestRecord>>(result);
    }

    [Fact]
    public void TestStore()
    {
        //Arrange
        string filePath ="../../../../data/testData.csv";
        var db = new CSVDatabase<TestRecord>(filePath);

        //Act
        db.ResetTestDB();
        db.Store(new TestRecord("yo"));

        //Assert
        var result = db.Read();
        Assert.Contains("yo", result.Select(r => r.Message));
        
    }

    public record TestRecord(string Message);

}