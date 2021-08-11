using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data.Models;

namespace WalksInNature.Test.Data
{
    public static class Walks
    {
        public static IEnumerable<Walk> TenPublicWalks
            => Enumerable.Range(0, 10).Select(i => new Walk
            {
                IsPublic = true
            });

        public static List<Walk> GetWalks(int count, bool isPublic = true, bool sameUser = true)
        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };

            var walks = Enumerable
                .Range(1, count)
                .Select(i => new Walk
                {
                    Id = i,
                    Name = $"Walk {i}",
                    ImageUrl = $"Walk ImageUrl {i}",
                    StartPoint = $"Walk StartPoint {i}",
                    RegionId = 1,
                    LevelId = 1,
                    Description = $"Walk Description {i}",
                    IsPublic = isPublic,
                    AddedByUser = sameUser ? user : new User
                    {
                        Id = $"Author Id {i}",
                        UserName = $"Author {i}"
                    }
                })
                .ToList();

            return walks;
        }
    }
}  
    