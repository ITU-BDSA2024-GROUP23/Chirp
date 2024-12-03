using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class CheepRepositoryTests
{
    private readonly ChirpDBContext _context;
    private readonly SqliteConnection _connection;
    private readonly CheepRepository _repository;
    private readonly UserRepository _userrepo;
    public CheepRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(_connection);

        _context = new ChirpDBContext(options.Options);
        _repository = new CheepRepository(_context);
        _userrepo = new UserRepository(_context);

        //IMPORTANT: Uncomment this line if we don't seed the db ChirpDBContext
        //DbInitializer.SeedDatabase(_context);
    }

    [Fact]
    public async Task TestGetCheeps_ReturnsNonEmpty()
    {
        var result = await _repository.GetCheeps(0);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task TestGetCheeps_DifferentPages()
    {
        var result = await _repository.GetCheeps(0);
        var result2 = await _repository.GetCheeps(1);
        Assert.NotEqual(result, result2);
    }

    [Fact]
    public async Task TestGetCheepsFromUserName_ReturnsNonEmpty()
    {
        var result = await _repository.GetCheepsFromUserName("Roger Histand", 0);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task TestGetCheepsFromUserName_ShouldReturnCheepsForUser()
    {
        var result = await _repository.GetCheepsFromUserName("Roger Histand", 0);
        var result2 = await _repository.GetCheepsFromUserName("Luanna Muro", 0);
        Assert.All(result, cheep => Assert.Equal("Roger Histand", cheep.Author));
        Assert.All(result2, cheep => Assert.Equal("Luanna Muro", cheep.Author));
    }

    [Fact]
    public async Task TestGetCheepsFromUserName_ShouldReturnCheepsInDescendingOrder()
    {
        var result = await _repository.GetCheepsFromUserName("Roger Histand", 0);
        Assert.True(DateTime.Parse(result[0].TimeStamp) > DateTime.Parse(result[1].TimeStamp));
    }

    [Fact]
    public async Task TestGetCheepsFromEmail_ReturnsNonEmpty()
    {
        var result = await _repository.GetCheepsFromEmail("Roger+Histand@hotmail.com", 0);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task TestGetUserFromString_ReturnsCorrectUser()
    {
        var result = await _userrepo.GetUserByString("Roger Histand");
        Assert.Equal("Roger Histand", result.UserName);
        Assert.Equal("Roger+Histand@hotmail.com", result.Email);
    }

    [Fact]
    public async Task TestGetUserFromString_Throws()
    {
        await Assert.ThrowsAsync<Exception>(async () => await _userrepo.GetUserByString("This user does not exist"));
    }

    [Fact]
    public async Task TestCreateCheep()
    {
        //Arrange
        var user = await _userrepo.GetUserObjectByString("Roger Histand");

        //Act
        await _repository.CreateCheep(user, "Test cheep");

        //Assert
        Assert.Contains(_context.Cheeps, cheep => cheep.Text == "Test cheep");
    }

    [Fact]
    public async Task TestGetNextCheepId_Increments()
    {
        //Arrange
        var user = await _userrepo.GetUserObjectByString("Roger Histand");
        var currentId = _repository.GetNextCheepId();

        //Act
        await _repository.CreateCheep(user, "Test cheep");

        //Assert
        Assert.Equal(currentId + 1, _repository.GetNextCheepId());
    }

    [Fact]
    public async Task TestNoFollowRelationship()
    {
        //Arrange
        var user1 = await _userrepo.GetUserByString("Roger Histand");
        var user2 = await _userrepo.GetUserByString("Luanna Muro");

        //Act
        var followers1 = await _userrepo.GetFollowers(user1);
        var followers2 = await _userrepo.GetFollowers(user2);

        //Assert
        Assert.DoesNotContain(followers1, user => user.UserName == "Luanna Muro");
        Assert.DoesNotContain(followers2, user => user.UserName == "Roger Histand");
    }

    [Fact]
    public async Task TestFollowUser()
    {
        //Arrange
        var user1 = await _userrepo.GetUserByString("Roger Histand");
        var user2 = await _userrepo.GetUserByString("Luanna Muro");

        //Act
        await _userrepo.FollowUser(user1, user2);
        await _userrepo.FollowUser(user2, user1);
        var followers1 = await _userrepo.GetFollowers(user1);
        var followers2 = await _userrepo.GetFollowers(user2);

        //Assert
        Assert.Contains(followers1, user => user.UserName == "Luanna Muro");
        Assert.Contains(followers2, user => user.UserName == "Roger Histand");
    }

    [Fact]
    public async Task TestUnfollowUser()
    {
        //Arrange
        var user1 = await _userrepo.GetUserByString("Roger Histand");
        var user2 = await _userrepo.GetUserByString("Luanna Muro");

        //Act
        await _userrepo.FollowUser(user1, user2);
        await _userrepo.FollowUser(user2, user1);
        await _userrepo.UnfollowUser(user1, user2);
        await _userrepo.UnfollowUser(user2, user1);
        var followers1 = await _userrepo.GetFollowers(user1);
        var followers2 = await _userrepo.GetFollowers(user2);

        //Assert
        Assert.DoesNotContain(followers1, user => user.UserName == "Luanna Muro");
        Assert.DoesNotContain(followers2, user => user.UserName == "Roger Histand");
    }

    [Fact]
    public async Task TestGetFollowing()
    {
        //Arrange
        var user1 = await _userrepo.GetUserByString("Roger Histand");
        var user2 = await _userrepo.GetUserByString("Luanna Muro");

        //Act
        await _userrepo.FollowUser(user1, user2);
        await _userrepo.FollowUser(user2, user1);
        var following1 = await _userrepo.GetFollowing(user1);
        var following2 = await _userrepo.GetFollowing(user2);

        //Assert
        Assert.Contains(following1, user => user.UserName == "Luanna Muro");
        Assert.Contains(following2, user => user.UserName == "Roger Histand");
    }

    [Fact]
    public async Task TestGetFollowers()
    {
        //Arrange
        var user1 = await _userrepo.GetUserByString("Roger Histand");
        var user2 = await _userrepo.GetUserByString("Luanna Muro");

        //Act
        await _userrepo.FollowUser(user1, user2);
        await _userrepo.FollowUser(user2, user1);
        var followers1 = await _userrepo.GetFollowers(user1);
        var followers2 = await _userrepo.GetFollowers(user2);

        //Assert
        Assert.Contains(followers1, user => user.UserName == "Luanna Muro");
        Assert.Contains(followers2, user => user.UserName == "Roger Histand");
    }

    [Fact]
    public async Task TestDeleteCheep()
    {
        //Arrange
        var user = await _userrepo.GetUserObjectByString("Roger Histand");
        await _repository.CreateCheep(user, "Test cheep");
        var cheep = _context.Cheeps.First(cheep => cheep.Text == "Test cheep");

        //Act
        await _repository.DeleteCheep(cheep.CheepId);

        //Assert
        Assert.DoesNotContain(_context.Cheeps, cheep => cheep.Text == "Test cheep");
    }
}
