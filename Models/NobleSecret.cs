using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace KingdomApi.Models
{
    [Table("noble_secrets")]
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(EmailAddress), IsUnique = true)]
    public class NobleSecret
    {
        [Key]
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Username { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public int NobleId { get; set; }
        public Noble Noble { get; set; }
    }
}
