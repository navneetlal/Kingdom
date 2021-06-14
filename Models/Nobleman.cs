using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KingdomApi.Models
{
    [Table("noblemen")]
    public class Nobleman
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint NoblemanId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Username { get; set; }

        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string FullName { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; } = Gender.PreferNotToSay;

        public string OrganizationName { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public string EmployeeId { get; set; }
        public string ReportingManager { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public uint PostalCode { get; set; }

        public ICollection<Responsibility> Responsibilities { get; set; }


        public ICollection<Clan> Clans { get; set; }

        public uint KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }
    }

    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male,

        [EnumMember(Value = "Female")]
        Female,

        [EnumMember(Value = "Other")]
        Other,

        [EnumMember(Value = "PreferNotToSay")]
        PreferNotToSay
    }
}
