using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data.Models;

namespace WalksInNature.Test.Data
{
    public class Users
    {
        public static IEnumerable<User> GetUsers(int count = 5)
            => Enumerable.Range(1, count).Select(i => new User()
            {

            });

        public static Guide GetGuide(string userId = TestUser.Identifier)
        {
            var guide = new Guide()
            {
                UserId = userId
            };

            return guide;
        }
    }
}
