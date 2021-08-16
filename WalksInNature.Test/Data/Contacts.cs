using System;
using WalksInNature.Data.Models;

namespace WalksInNature.Test.Data
{
    public class Contacts
    {
        public static ContactForm GetContact(int id = 1, bool IsReplied = false)
        {
           
            return new ContactForm
            {
                Id = id,
                Name = "Sender",
                Email = "test@abv.bg",
                IsReplied = IsReplied,
                Subject = "TestSubject",
                Message = "TetsMessage",
                CreatedOn = new DateTime(2022,1,1)
            };
        }
    }
}
