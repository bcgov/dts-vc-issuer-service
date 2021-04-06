using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Prime.Models
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

        public ICollection<PatientCredential> PatientCredentials { get; set; }

        // [NotMapped]
        // public string Base64QRCode
        // {
        //     get => PatientCredentials?
        //         .OrderByDescending(s => s.Id)
        //         .Select(ec => ec.Credential?.Base64QRCode)
        //         .FirstOrDefault();
        // }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Guid.Empty.Equals(UserId))
            {
                yield return new ValidationResult($"UserId cannot be empty");
            }
        }
    }
}
