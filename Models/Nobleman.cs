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
        public UInt64 NoblemanId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public String Username { get; set; }

        public String Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public String FullName { get; set; }

        [EmailAddress]
        public String EmailAddress { get; set; }

        [Phone]
        public String PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; } = Gender.RatherNotDisclose;

        public String OrganizationName { get; set; }
        public String Department { get; set; }
        public String JobTitle { get; set; }
        public String EmployeeId { get; set; }
        public String ReportingManager { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String Country { get; set; }
        public UInt32 PostalCode { get; set; }

        public ICollection<Responsibility> Responsibilities { get; set; }
        public ICollection<Clan> clans { get; set; }

        public UInt64 KingdomId { get; set; }
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

        [EnumMember(Value = "RatherNotDisclose")]
        RatherNotDisclose
    }
}
