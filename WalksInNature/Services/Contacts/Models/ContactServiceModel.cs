using System;

namespace WalksInNature.Services.Contacts.Models
{
    public class ContactServiceModel
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnFormated => this.CreatedOn.Date.ToString("dd.MM.yyyy");
        public bool IsReplied { get; set; }
    }
}
