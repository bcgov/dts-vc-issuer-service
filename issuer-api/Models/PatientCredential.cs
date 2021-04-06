using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Prime.Models
{
    [Table("PatientCredential")]
    public class PatientCredential : BaseAuditable
    {
        [Key]
        public int Id { get; set; }

        [JsonIgnore]
        public int PatientId { get; set; }

        [JsonIgnore]
        public Patient Patient { get; set; }

        public int CredentialId { get; set; }

        [JsonIgnore]
        public Credential Credential { get; set; }
    }
}
