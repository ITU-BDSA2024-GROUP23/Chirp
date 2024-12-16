using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Tests;

public class ModelsTest
{
    [Fact]
    public void UserCanBeInstantiated()
    {
        // Arrange
        User user = new User { UserName = "Victor", Email = "vmem@itu.dk"};

        // Assert
        Assert.Equal("Victor", user.UserName);
        Assert.Equal("vmem@itu.dk", user.Email);
    }

    [Fact]
    public void CheepCanBeInstantiated()
    {
        // Arrange
        User user = new User { UserName = "Victor", Email = "vmem@itu.dk"};
        Cheep cheep = new Cheep { CheepId = 1, Author = user, Text = "test", TimeStamp = DateTime.Now };

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
            Author = new User { UserName = "Victor", Email = "vmem@itu.dk"},
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
            Author = new User { UserName = "Victor", Email = "vmem@itu.dk"},
            Text = "",
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
