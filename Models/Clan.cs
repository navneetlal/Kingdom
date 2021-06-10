using System;
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
        public UInt32 ClanId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public String ClanName { get; set; }

        public String ClanPurpose { get; set; }

        public ICollection<Nobleman> Noblemen { get; set; }
        public ICollection<Responsibility> Responsibilities { get; set; }

        public UInt32 KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }
    }
}
