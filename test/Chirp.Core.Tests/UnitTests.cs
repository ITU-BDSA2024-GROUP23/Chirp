namespace Chirp.Core.Tests;

public class UnitTest1
{
    [Fact]
    public void AuthorCanBeInstantiated()
    {
        // Arrange
        Author author = new Author { Name = "Victor", Email = "vmem@itu.dk" };

        // Assert
        Assert.Equal("Victor", author.Name);
        Assert.Equal("vmem@itu.dk", author.Email);
        Assert.Empty(author.Cheeps);
    }

    [Fact]
    public void CheepCanBeInstantiated()
    {
        // Arrange
        Author author = new Author { Name = "Victor", Email = "vmem@itu.dk" };
        Cheep cheep = new Cheep { CheepId = 1, Author = author, AuthorId = author.AuthorId, Text = "test", TimeStamp = DateTime.Now };

        // Assert
        Assert.Equal("test", cheep.Text);
    }
}