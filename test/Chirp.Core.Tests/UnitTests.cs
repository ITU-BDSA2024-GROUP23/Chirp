using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Tests;

public class UnitTests
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

    // https://medium.com/@bimsara.gunarathna/write-unit-tests-for-data-annotation-validator-attributes-in-net-ba9546e95f
    [Fact]
    public void CheepTextCannotBeEmpty()
    {
        //Arrange
        Cheep cheep = new Cheep
        {
            CheepId = 1,
            Author = new Author { AuthorId = 1, Name = "Victor", Email = "vmem@itu.dk" },
            AuthorId = 1,
            Text = "",
            TimeStamp = DateTime.Now
        };

        ValidationContext validationContext = new ValidationContext(cheep);
        List<ValidationResult> validationResults = new List<ValidationResult>();

        // Act
        bool empty = Validator.TryValidateObject(cheep, validationContext, validationResults, true);

        // Assert
        Assert.False(empty);
    }

    [Fact]
    public void CheepTextCannotBeLongerThan160Characters()
    {
        //Arrange
        Cheep cheep = new Cheep
        {
            CheepId = 1,
            Author = new Author { AuthorId = 1, Name = "Victor", Email = "vmem@itu.dk" },
            AuthorId = 1,
            Text = new('a', 161),
            TimeStamp = DateTime.Now
        };

        ValidationContext validationContext = new ValidationContext(cheep);
        List<ValidationResult> validationResults = new List<ValidationResult>();

        // Act
        bool tooLong = Validator.TryValidateObject(cheep, validationContext, validationResults, true);

        // Assert
        Assert.False(tooLong);
    }
}
