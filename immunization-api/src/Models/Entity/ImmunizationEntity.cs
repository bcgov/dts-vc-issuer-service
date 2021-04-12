namespace ImmunizationApi.Models.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents basic Patient or Subject Information
    /// </summary>
    public class ImmunizationEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime AdministeredOnDate { get; set; }
        public DateTime NextDueDate { get; set; }
        public string LotNumber { get; set; }
        public int DoseNumber { get; set; }
        public string PatientId { get; set; }

        public string VaccineId { get; set; }

        public int LocationId { get; set; }

        [ForeignKey("PatientId")]
        public virtual PatientEntity Patient { get; set; }

        [ForeignKey("VaccineId")]
        public virtual VaccineEntity Vaccine { get; set; }

        [ForeignKey("LocationId")]
        public virtual LocationEntity AdministeredAt { get; set; }
    }
}