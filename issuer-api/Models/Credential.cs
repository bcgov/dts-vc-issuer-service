using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Issuer.Models
{
    [Table("Credential")]
    public class Credential : BaseAuditable
    {
        [Key]
        public int Id { get; set; }

        public int ConnectionId { get; set; }

        [JsonIgnore]
        public Connection Connection { get; set; }

        public int IdentifierId { get; set; }

        [JsonIgnore]
        public Identifier Identifier { get; set; }

        public string SchemaId { get; set; }

        public string CredentialExchangeId { get; set; }

        public string CredentialDefinitionId { get; set; }

        public DateTimeOffset? AcceptedCredentialDate { get; set; }

        public DateTimeOffset? RevokedCredentialDate { get; set; }
    }
}
