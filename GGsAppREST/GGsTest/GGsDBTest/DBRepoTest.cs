using GGsDB.Entities;
using GGsDB.Mappers;
using GGsDB.Models;
using GGsDB.Repos;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace GGsTest.GGsDBTest
{

    public class DBRepoTest
    {
        DBRepo repo;
        readonly DBMapper mapper = new DBMapper();
        private readonly Location testLocation = new Location()
        {
            id = 10,
            street = "772 Bat Dr",
            city = "San Jose",
            state = "CA",
            zipCode = "94541"
        };

        private readonly User testUser = new User()
        {
            id = 1,
            name = "Jacob",
            email = "jjj@gmail.com",
            locationId = 1,
            type = User.userType.Customer
        };

        [Fact]
        public void AddUserShouldAdd()
        {
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("AddUserShouldAdd").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddUser(testUser);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Users.Single(u => u.Name == testUser.name));
        }
        [Fact]
        public void GetUserShouldGet()
        {
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("AddUserShouldAdd").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);
        }
    }
}