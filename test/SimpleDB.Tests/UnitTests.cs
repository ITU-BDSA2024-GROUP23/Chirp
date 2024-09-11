namespace SimpleDB.Tests;

public class UnitTests
{
    private readonly static string filePath = "../data/testData.csv";
    [Fact]
    public void TestFileNotFound() 
    {
        //Arrange
        string path = "notfound.csv";
        
        //Assert
        Assert.Throws<FileNotFoundException>(() => new CSVDatabase<TestRecord>(path));
    }

    [Fact]
    public void TestRead()
    {
        //Arrange
        var db = new CSVDatabase<TestRecord>(filePath);

        //Act
        var result = db.Read();

        //Assert
        Assert.IsType<List<TestRecord>>(result);
        
    }

    public record TestRecord(int id, string name)
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}