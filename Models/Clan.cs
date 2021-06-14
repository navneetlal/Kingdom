using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KingdomApi.Models
{
    [Table("clans")]
    public class Clan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint ClanId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string ClanName { get; set; }

        public string ClanPurpose { get; set; }

        public ICollection<Nobleman> Noblemen { get; set; }
        public ICollection<Responsibility> Responsibilities { get; set; }

        public uint KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }
    }
}
