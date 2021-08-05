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
        public Guid ClanId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string ClanName { get; set; }

        public string ClanPurpose { get; set; }

        public ICollection<Noble> Nobles { get; set; }
        public ICollection<Responsibility> Responsibilities { get; set; }

        public Guid KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }
    }
}
