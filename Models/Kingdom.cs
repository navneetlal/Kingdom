using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KingdomApi.Models
{
    [Table("kingdoms")]
    public class Kingdom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KingdomId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string KingdomName { get; set; }

        public string Description { get; set; }

        public List<Clan> Clans { get; set; }
        public List<Noble> Nobles { get; set; }
        public List<Responsibility> Responsibilities { get; set; }
    }
}
