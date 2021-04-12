namespace ImmunizationApi.Models.Entity
{
        using System.ComponentModel.DataAnnotations;

    public class VaccineEntity
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Disease { get; set; }
    }
}
