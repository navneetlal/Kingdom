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
        public UInt64 ClanId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public String ClanName { get; set; }
        
        public String ClanPurpose { get; set; }

        public List<Nobleman> Noblemen { get; set; }
        public List<Responsibility> Responsibilities { get; set; }
    }
}