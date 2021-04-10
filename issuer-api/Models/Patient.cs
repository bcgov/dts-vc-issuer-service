using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Issuer.Models
{
    [Table("Patient")]
    public class Patient : BaseAuditable, IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        public Guid UserId { get; set; }

        [StringLength(255)]
        public string HPDID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string GivenNames { get; set; }

        public string PreferredFirstName { get; set; }

        public string PreferredMiddleName { get; set; }

        public string PreferredLastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [JsonIgnore]
        public ICollection<Connection> Connections { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Guid.Empty.Equals(UserId))
            {
                yield return new ValidationResult($"UserId cannot be empty");
            }
        }
    }
}
