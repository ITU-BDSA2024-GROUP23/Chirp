namespace SimpleDB.Tests;

public class UnitTests
{
    /*
    [Fact]
    public void TestFileNotFound()
    {
        //Arrange
        string path = "notfound.csv";
        Console.WriteLine(Environment.CurrentDirectory);

        //Assert
        Assert.Throws<FileNotFoundException>(() => CSVDatabase<TestRecord>.GetInstance(path));
    }

    [Fact]
    public void TestRead()
    {
        //Arrange
        string filePath = "../../../../data/testData.csv";
        var db = CSVDatabase<TestRecord>.GetInstance(filePath);

        //Act
        var result = db.Read();

        //Assert
        Assert.IsType<List<TestRecord>>(result);
    }

    [Fact]
    public void TestStoreThrows()
    {
        string filePath = "../../../../data/testData.csv";
        var db = CSVDatabase<TestRecord>.GetInstance(filePath);
        Assert.Throws<ArgumentNullException>(() => db.Store(null));
    }

    [Fact]
    public void TestResetDB()
    {
        //Arrange
        string filePath = "../../../../data/testData.csv";
        var db = CSVDatabase<TestRecord>.GetInstance(filePath);

        //Act
        db.ResetTestDB();

        //Assert
        using StreamReader sr = new StreamReader(filePath);
        Assert.Equal("Message", sr.ReadLine());
        Assert.Null(sr.ReadLine());
    }

    [Fact]
    public void TestSingleton()
    {
        //Arrange
        string path = "../../../../data/testData.csv";

        //Act
        var instance1 = CSVDatabase<TestRecord>.GetInstance(path);
        var instance2 = CSVDatabase<TestRecord>.GetInstance(path);

        //Assert
        Assert.NotNull(instance1);
        Assert.NotNull(instance2);
        Assert.Same(instance1, instance2);
    }

    public record TestRecord(string Message);
    */

}
