using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Issuer.Models
{
    [Table("Identifier")]
    public class Identifier : BaseAuditable
    {
        [Key]
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Uri { get; set; }
    }
}
