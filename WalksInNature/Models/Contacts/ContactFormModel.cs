using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.ContactForm;

namespace WalksInNature.Models.Contacts
{
    public class ContactFormModel
    {               

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "The {0} can be between {2} and {1} characters")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(SubjectMaxLength, MinimumLength = SubjectMinLength, ErrorMessage = "The {0} can be between {2} and {1} characters")]        
        public string Subject { get; set; }

        [Required]        
        [StringLength(MessageMaxLength, MinimumLength = MessageMinLength, ErrorMessage = "The {0} can be between {2} and {1} characters")]
        public string Message { get; set; }
    }
}
