using System;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data.Models;

namespace WalksInNature.Test.Data
{
    public class Contacts
    {
        public static IEnumerable<ContactForm> GetContacts(int count = 5)
          => Enumerable.Range(1, count).Select(i => new ContactForm()
          {

          });

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
