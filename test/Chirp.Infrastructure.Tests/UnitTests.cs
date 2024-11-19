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
            var repository = new CheepRepository(context);

            var result = await repository.GetCheepsFromUserName("Roger Histand", 0);

            Assert.All(result, cheep => Assert.Equal("Roger Histand", cheep.Author));        }
    }
}
