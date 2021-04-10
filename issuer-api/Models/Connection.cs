using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Issuer.Models
{
    [Table("Connection")]
    public class Connection : BaseAuditable
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }

        [JsonIgnore]
        public Patient Patient { get; set; }

        public string ConnectionId { get; set; }

        public string Base64QRCode { get; set; }

        public DateTimeOffset? AcceptedConnectionDate { get; set; }

        public ICollection<Credential> Credentials { get; set; }

    }
}
