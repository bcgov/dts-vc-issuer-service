namespace ImmunizationApi.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class LocationEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetLine1 { get; set; }
        public string StreetLine2 { get; set; }
        public string City { get; set; }
    
        [Required]
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}