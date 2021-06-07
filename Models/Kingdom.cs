using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KingdomApi.Models
{
    [Table("kingdoms")]
    public class Kingdom
    {
        [Key]
        public UInt64 KingdomId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public String KingdomName { get; set; }
        
        public String Description { get; set; }

        public List<Clan> Clans { get; set; }
        public List<Nobleman> Noblemen { get; set; }
    }
}