using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class CheepRepositoryTests
{
    [Fact]
    public async Task GetCheepsFromUserName_ShouldReturnCheepsForSpecifiedUser()
    {
        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using (var context = new ChirpDBContext(options))
        {
            var user1 = new User
            {
                UserName = "User1",
                Email = "user1@test.com",
                Following = new List<Follower>(),
                Followers = new List<Follower>()
            };

            var user2 = new User
            {
                UserName = "User2",
                Email = "user2@test.com",
                Following = new List<Follower>(),
                Followers = new List<Follower>()
            };

            context.Users.AddRange(user1, user2);

            for (int i = 1; i <= 50; i++)
            {
                context.Cheeps.Add(new Cheep
                {
                    CheepId = i,
                    Author = (i % 2 == 0) ? user1 : user2,
                    Text = $"Cheep {i}",
                    TimeStamp = DateTime.Now.AddMinutes(-i)
                });
            }

            await context.SaveChangesAsync();
        }

        using (var context = new ChirpDBContext(options))
        {
            var repository = new CheepRepository(context);

            var result = await repository.GetCheepsFromUserName("User1", 0);

            Assert.All(result, cheep => Assert.Equal("User1", cheep.Author));
            Assert.True(result.First().TimeStamp.CompareTo(result.Last().TimeStamp) >= 0); 
        }
    }
}
