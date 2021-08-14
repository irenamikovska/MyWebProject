using AutoMapper;
using System;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Models.Contacts;

namespace WalksInNature.Services.Contacts
{
    public class ContactService : IContactService
    {
        private readonly WalksDbContext data;
        private readonly IMapper mapper;
        public ContactService(WalksDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }
          
        public int EntryContactForm(ContactFormModel inputModel)
        {
            var contactFormEntry = new ContactForm
            {
                Name = inputModel.Name,
                Email = inputModel.Email,
                Subject = inputModel.Subject,
                Message = inputModel.Message,
                CreatedOn = DateTime.UtcNow                               
            };

            this.data.ContactForms.Add(contactFormEntry);
            this.data.SaveChanges();
            
            return contactFormEntry.Id;
        }

        public ContactConfirmMessageViewModel ConfirmMessage(string senderName)
            => new ContactConfirmMessageViewModel
            {
                SenderName = senderName
            };
    }
}
