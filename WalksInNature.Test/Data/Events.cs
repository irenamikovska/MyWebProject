using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data.Models;
using static WalksInNature.Test.Data.DataConstants;

namespace WalksInNature.Test.Data
{
    public class Events
    {
        public static IEnumerable<Event> TenPublicEvents
            => Enumerable.Range(1, 10).Select(i => new Event
            {
                IsPublic = true
            });


        public static List<Event> GetEvents(int count = 5, bool isPublic = false, bool sameUser = true)
        {
            
            var guide = new Guide
            {

                Name = TestUser.Username,
                UserId = TestUser.Identifier,               
            };

            var events = Enumerable
                .Range(1, count)
                .Select(i => new Event
                {
                    Id = i,
                    Name = $"Name {i}",
                    ImageUrl = $"ImageUrl {i}",
                    StartPoint = $"StartPoint {i}",
                    Date = new DateTime(2022, 1, 1),
                    StartHour = new TimeSpan(08, 00, 00),
                    IsPublic = isPublic ? false : true,
                    Guide = sameUser ? guide : new Guide
                    {
                        Id = i + 100,
                        Name = $"Author {i}"
                    },
                    Description = $"Description {i}",
                    /*Region = new Region
                    {
                        Id = 1,
                        Name = "TestRegion"
                    },
                    Level = new Level
                    {
                        Id = 1,
                        Name = "TestLevel"
                    }  */                

                })
                .ToList();

            return events;
    }

         public static Event GetEvent(int id = TestIdInt, bool isPublic = true, bool userSame = true)
         {

             var user = new User
             {
                 Id = TestUser.Identifier,
                 UserName = TestUser.Username,
             };


             var guide = new Guide
             {

                 Name = TestUser.Username,
                 User = userSame ? user : new User
                 {
                     Id = "DifferentId",
                     UserName = "DifferentName"
                 },
                 UserId = userSame ? TestUser.Identifier : "DifferentId"
             };

             return new Event
             {
                 Id = id,
                 IsPublic = isPublic,
                 Guide = guide,
                 Region = new Region
                 {
                     Id = 1,
                     Name = "TestRegion"
                 },
                 Level = new Level
                 {
                     Id = 1,
                     Name = "TestLevel"
                 },

                 Description = "TestDescription",
                 Users = new List<EventUser>()
                 {
                     new EventUser()
                     {
                         UserId = "TestUserId",
                         EventId = TestIdInt
                     }
                 }
             };

           

        }
    }
}

