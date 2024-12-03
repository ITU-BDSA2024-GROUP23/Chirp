using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class CheepServiceTests
{
    private readonly ChirpDBContext _context;
    private readonly SqliteConnection _connection;
    private readonly CheepService _service;
    private readonly UserRepository _userRepo;

    public CheepServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(_connection);

        _context = new ChirpDBContext(options.Options);
        _userRepo = new UserRepository(_context);
        var cheepRepository = new CheepRepository(_context);
        _service = new CheepService(cheepRepository);
    }

    [Fact]
    public async Task TestDifferentPages()
    {
        // Act
        var result = await _service.GetCheeps(0);
        var result2 = await _service.GetCheeps(1);

        // Assert
        Assert.NotEqual(result, result2);
    }

    [Fact]
    public async Task TestGetCheepsFromUserName()
    {
        // Act
        var result = await _service.GetCheepsFromUserName("Roger Histand", 0);

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task TestAllCheepsBelongToUser()
    {
        // Act
        var result = await _service.GetCheepsFromUserName("Roger Histand", 0);

        // Assert
        Assert.All(result, cheep => Assert.Equal("Roger Histand", cheep.Author));
    }

    [Fact]
    public async Task TestGetCheepsFromEMail()
    {
        // Act
        var result = await _service.GetCheepsFromEmail("Roger+Histand@hotmail.com", 0);

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task TestCreateCheep()
    {
        // Arrange
        var user = await _userRepo.GetUserObjectByString("Roger Histand");

        // Act
        await _service.CreateCheep(user, "Test cheep");

        // Assert
        Assert.Contains(_context.Cheeps, cheep => cheep.Text == "Test cheep");
    }

    [Fact]
    public async Task TestDeleteCheep()
    {
        // Arrange
        var user = await _userRepo.GetUserObjectByString("Roger Histand");
        await _service.CreateCheep(user, "Test cheep");
        var cheep = _context.Cheeps.First(cheep => cheep.Text == "Test cheep");

        // Act
        await _service.DeleteCheep(cheep.CheepId);

        // Assert
        Assert.DoesNotContain(_context.Cheeps, cheep => cheep.Text == "Test cheep");
    }

    [Fact]
    public async Task TestCannotDeleteNonExistentCheep()
    {
        // Act
        var success = await _service.DeleteCheep(-1);

        // Assert
        Assert.False(success);
    }
}