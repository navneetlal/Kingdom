using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KingdomApi.Models
{
    [Table("responsibilities")]
    public class Responsibility
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResponsibilityId { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string ResponsibilityName { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Target { get; set; }

        [Required]
        [StringLength(24, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Action { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Column(TypeName = "varchar(24)")]
        public Depth Depth { get; set; } = Depth.Own;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Column(TypeName = "varchar(24)")]
        public Effect Effect { get; set; } = Effect.Permit;

        [Column(TypeName = "jsonb")]
        public JsonDocument Condition { get; set; }

        [Column(TypeName = "jsonb")]
        public Obligation Obligation { get; set; }

        [Range(1, 100, ErrorMessage = "{0} must be between {1} and {2}.")]
        public short Priority { get; set; } = 1;

        public ICollection<Clan> Clans { get; set; }
        public ICollection<Noble> Nobles { get; set; }

        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }
    }

    public class Obligation
    {
        public JsonDocument Permit { get; set; }
        public JsonDocument Deny { get; set; }
    }

    public enum Depth
    {
        [EnumMember(Value = "Own")]
        Own,

        [EnumMember(Value = "Clan")]
        Clan,

        [EnumMember(Value = "Kingdom")]
        Kingdom
    }

    public enum Effect
    {
        [EnumMember(Value = "Permit")]
        Permit,

        [EnumMember(Value = "Deny")]
        Deny
    }
}
