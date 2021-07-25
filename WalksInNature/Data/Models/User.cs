using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.User;

namespace WalksInNature.Data.Models
{
    public class User : IdentityUser
    {
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }
    }
}
