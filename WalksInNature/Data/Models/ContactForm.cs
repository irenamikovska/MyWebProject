using System;
using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.ContactForm;

namespace WalksInNature.Data.Models
{
    public class ContactForm
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]        
        public string Email { get; set; }

        [Required]     
        [MaxLength(SubjectMaxLength)]
        public string Subject { get; set; }

        [Required]        
        [MaxLength(MessageMaxLength)]
        public string Message { get; set; }

        public bool IsReplied { get; set; }
    }
}
