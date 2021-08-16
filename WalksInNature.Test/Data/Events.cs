using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data.Models;
using static WalksInNature.Test.Data.DataConstants;

namespace WalksInNature.Test.Data
{
    public class Events
    {       
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

