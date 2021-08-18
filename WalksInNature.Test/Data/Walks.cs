using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data.Models;

using static WalksInNature.Test.Data.DataConstants;

namespace WalksInNature.Test.Data
{
    public static class Walks
    {
        public static IEnumerable<Walk> TenPublicWalks
            => Enumerable.Range(1, 10).Select(i => new Walk
            {
                IsPublic = true
            });

        public static List<Walk> GetWalks(int count = 5, bool isPublic = false, bool sameUser = true)
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
                    Name = $"Walk Name {i}",
                    ImageUrl = $"Walk ImageUrl {i}",
                    StartPoint = $"Walk StartPoint {i}",
                    RegionId = 1,
                    LevelId = 1,
                    IsPublic = isPublic ? false : true,
                    Description = $"Walk Description {i}",
                    AddedByUser = sameUser ? user : new User
                    {
                        Id = $"Author Id {i}",
                        UserName = $"Author {i}"
                    }
                })
                .ToList();

            return walks;
        }

        public static Walk GetWalk(int id = 1, bool IsPublic = false)
        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };

            return new Walk
            {
                Id = id,               
                IsPublic = IsPublic ? false : true,
                AddedByUserId = user.Id,
                Region = new Region
                {
                    Id = 1,
                    Name = "TestRegion"
                },
                Level = new Level
                {
                    Id = 1,
                    Name = "TestLevel"
                }   
            };
        }
    }
}  
    