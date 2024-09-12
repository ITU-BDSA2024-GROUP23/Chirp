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
        string filePath = "../../../../data/testData.csv";
        var db = new CSVDatabase<TestRecord>(filePath);

        //Act
        var result = db.Read();

        //Assert
        Assert.IsType<List<TestRecord>>(result);
    }

    [Fact]
    public void TestStoreThrows()
    {
        string filePath = "../../../../data/testData.csv";
        var db = new CSVDatabase<TestRecord>(filePath);
        Assert.Throws<ArgumentNullException>(() => db.Store(null));
    }

    [Fact]
    public void TestResetDB()
    {
        //Arrange
        string filePath = "../../../../data/testData.csv";
        var db = new CSVDatabase<TestRecord>(filePath);

        //Act
        db.ResetTestDB();

        //Assert
        using StreamReader sr = new StreamReader(filePath);
        Assert.Equal("Message", sr.ReadLine());
        Assert.Null(sr.ReadLine());
    }

    public record TestRecord(string Message);

}