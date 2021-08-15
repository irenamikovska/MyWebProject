using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Models.Contacts;
using WalksInNature.Services.Contacts.Models;

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
        public ContactQueryServiceModel All(
                    string sender = null,
                    string searchTerm = null,
                    MessageSorting sorting = MessageSorting.DateCreated,
                    int currentPage = 1,
                    int messagesPerPage = int.MaxValue)
        {
            var messagesQuery = this.data.ContactForms.AsQueryable();                

            if (!string.IsNullOrWhiteSpace(sender))
            {
                messagesQuery = messagesQuery.Where(x => x.Name == sender);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                messagesQuery = messagesQuery.Where(x =>
                    x.Message.ToLower().Contains(searchTerm.ToLower()) ||
                    x.Subject.ToLower().Contains(searchTerm.ToLower()) ||
                    x.Name.ToLower().Contains(searchTerm.ToLower()) ||
                    x.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            messagesQuery = sorting switch
            {
                MessageSorting.Email => messagesQuery.OrderBy(x => x.Email),
                MessageSorting.Subject => messagesQuery.OrderBy(x => x.Subject),                
                MessageSorting.DateCreated or _ => messagesQuery.OrderByDescending(x => x.Id)
            };

            var totalMessages = messagesQuery.Count();

            var messages = GetMessages(messagesQuery
                .Skip((currentPage - 1) * messagesPerPage)
                .Take(messagesPerPage));


            return new ContactQueryServiceModel
            {
                TotalMessages = totalMessages,
                MessagesPerPage = messagesPerPage,
                CurrentPage = currentPage,
                Messages = messages
            };
        }

        public IEnumerable<string> AllSenders()
            => this.data
                .ContactForms
                .Select(x => x.Name)
                .Distinct()
                .OrderBy(n => n)
                .ToList();
        
        public ContactDetailsServiceModel GetDetails(int messageId)

            => this.data
               .ContactForms
               .Where(x => x.Id == messageId)
               .Select(x => new ContactDetailsServiceModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   Email = x.Email,
                   Subject = x.Subject,
                   Message = x.Message,
                   CreatedOn = x.CreatedOn,
                   IsReplied = x.IsReplied
               })
               .FirstOrDefault();

        public int EntryContactForm(ContactFormModel inputModel)
        {
            var contactFormEntry = new ContactForm
            {
                Name = inputModel.Name,
                Email = inputModel.Email,
                Subject = inputModel.Subject,
                Message = inputModel.Message,
                CreatedOn = DateTime.UtcNow,
                IsReplied = false
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

        public void ChangeStatus(int messageId)
        {
            var message = this.data.ContactForms.Find(messageId);

            message.IsReplied = !message.IsReplied;

            this.data.SaveChanges();
        }

        private IEnumerable<ContactServiceModel> GetMessages(IQueryable<ContactForm> messageQuery)
           => messageQuery
                .ProjectTo<ContactServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();
    }
}
