using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

public class UserServiceTests
{
    private readonly ChirpDBContext _context;
    private readonly SqliteConnection _connection;
    private readonly UserService _userService;
    private readonly UserRepository _userRepo;

    public UserServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(_connection);

        _context = new ChirpDBContext(options.Options);
        _userRepo = new UserRepository(_context);
        var userRepository = new CheepRepository(_context);
        _userService = new UserService(userRepository);
    }

    [Fact]
    public async Task TestGetUserInfoDTO()
    {
        // Arrange
        string userStr = "Roger Histand";

        // Act
        var result = await _userService.GetUserInfoDTO(userStr);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Roger Histand", result.UserName);
        Assert.Equal("Roger+Histand@hotmail.com", result.Email);
        Assert.NotNull(result.Cheeps);
        Assert.NotNull(result.Followers);
        Assert.NotNull(result.Following);
    }

    [Fact]
    public async Task TestGetUserInfoDTO_Null()
    {
        // Arrange
        string userStr = "nonexistentuser";

        // Act
        var result = await _userService.GetUserInfoDTO(userStr);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task TestFollowUser_Null()
    {
        // Arrange
        var follower = await _userService.GetUserByString("Roger Histand");
        UserDTO followee = null;

        // Act
        var result = await _userService.FollowUser(follower, followee);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task TestFollowYourself()
    {
        // Arrange
        var user = await _userService.GetUserByString("Roger Histand");

        // Act
        var result = await _userService.FollowUser(user, user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task TestFollowUser()
    {
        // Arrange
        var follower = await _userService.GetUserByString("Roger Histand");
        var followee = await _userService.GetUserByString("Luanna Muro");

        // Act
        var result = await _userService.FollowUser(follower, followee);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TestUnfollowUser()
    {
        // Arrange
        var follower = await _userService.GetUserByString("Roger Histand");
        var followee = await _userService.GetUserByString("Luanna Muro");

        // Act
        await _userService.FollowUser(follower, followee);
        var result = await _userService.UnfollowUser(follower, followee);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TestDeleteNullUser()
    {
        // Arrange
        UserDTO user = null;

        // Act
        var result = await _userService.DeleteUser(user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task TestDeleteUser()
    {
        // Arrange
        var user = await _userService.GetUserByString("Roger Histand");

        // Act
        var result = await _userService.DeleteUser(user);

        // Assert
        Assert.True(result);
        var UserGone = await _userService.GetUserInfoDTO("Roger Histand");
        Assert.Null(UserGone);
    }
}
