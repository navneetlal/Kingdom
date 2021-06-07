using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KingdomApi.Models
{
    [Table("responsibilities")]
    public class Responsibility
    {
        [Key]
        public UInt64 ResponsibilityId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public String ResponsibilityName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public String ResourceName { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public String Action { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ActionLevel ActionLevel { get; set; } = ActionLevel.Own;
    }

    public enum ActionLevel
    {
        [EnumMember(Value = "Own")]
        Own,

        [EnumMember(Value = "Clan")]
        Clan,

        [EnumMember(Value = "Kingdom")]
        Kingdom,

        [EnumMember(Value = "All")]
        All
    }
}