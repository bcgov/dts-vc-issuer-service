
namespace ImmunizationApi.Repository.Example
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using ImmunizationApi.Models.Entity;

    public static class SampleDataInitializer
    {
        public static void Seed(ImmunizationDbContext context)
        {
            LocationEntity loc1 = new LocationEntity
            {
                Name = "VCHA",
                City = "Vancouver",
                Country = "CA",
                PostalCode = "V8A 0K0"

            };
            context.Locations.Add(loc1);

            LocationEntity loc2 = new LocationEntity
            {
                Name = "VCHA",
                City = "Vancouver",
                Country = "CA",
                PostalCode = "V8A 0K0"

            };
            context.Locations.Add(loc2);
            context.SaveChanges();

            PatientEntity patient1 = new PatientEntity
            {
                BirthDate = new System.DateTime(1957, 6, 22),
                GivenNames = "Johnny Michael",
                LastName = "Rose",
                Id = "9039555099"
            };
            context.Patients.Add(patient1);

            PatientEntity patient2 = new PatientEntity
            {
                BirthDate = new System.DateTime(1961, 3, 17),
                GivenNames = "Moira Maria",
                LastName = "Rose",
                Id = "9034545122"
            };
            context.Patients.Add(patient2);

            PatientEntity patient3 = new PatientEntity
            {
                BirthDate = new System.DateTime(1952, 1, 14),
                GivenNames = "Roland Ethan",
                LastName = "Schitt",
                Id = "900489178"
            };
            context.Patients.Add(patient3);

            PatientEntity patient4 = new PatientEntity
            {
                BirthDate = new System.DateTime(1991, 9, 22),
                GivenNames = "Veronica",
                LastName = "Lee",
                Id = "9902489314"
            };
            context.Patients.Add(patient4);
            context.SaveChanges();

            VaccineEntity vaccine1 = new VaccineEntity
            {
                Id = "02510014",  // DIN
                Name = "MODERNA COVID-19 mRNA-1273",
                Manufacturer = "Moderna Therapeutics Inc.",
                Disease = "COVID-19"
            };
            context.Vaccines.Add(vaccine1);

            VaccineEntity vaccine2 = new VaccineEntity
            {
                Id = "02510847",  // DIN
                Name = "ASTRAZENECA COVID-19 VACCINE (COVID-19)",
                Manufacturer = "AstraZeneca Canada Inc.",
                Disease = "COVID-19"
            };
            context.Vaccines.Add(vaccine2);

            VaccineEntity vaccine3 = new VaccineEntity
            {
                Id = "02509210",  // DIN
                Name = "PFIZER-BIONTECH COVID-19 VACCINE mRNA (COVID-19)",
                Manufacturer = "BioNTech Manufacturing GmbH",
                Disease = "COVID-19"
            };
            context.Vaccines.Add(vaccine3);

            VaccineEntity vaccine4 = new VaccineEntity
            {
                Id = "02512947",  // DIN
                Name = "COVISHIELD (COVID-19)",
                Manufacturer = "Verity Pharmaceuticals Inc.",
                Disease = "COVID-19"
            };
            context.Vaccines.Add(vaccine4);

            VaccineEntity vaccine5 = new VaccineEntity
            {
                Id = "02513153",  // DIN
                Name = "JANSSEN COVID-19 VACCINE (COVID-19)",
                Manufacturer = "Janssen Inc.",
                Disease = "COVID-19"
            };
            context.Vaccines.Add(vaccine5);
            context.SaveChanges();

            ImmunizationEntity imm1 = new ImmunizationEntity
            {
                AdministeredOnDate = new DateTime(2021, 1, 31),
                AdministeredAt = loc1,
                DoseNumber = 1,
                LotNumber = "123456A",
                NextDueDate = new DateTime(2021, 4, 30),
                PatientId = patient1.Id,
                VaccineId = vaccine1.Id
            };
            context.Immunizations.Add(imm1);

            ImmunizationEntity imm2 = new ImmunizationEntity
            {
                AdministeredOnDate = new DateTime(2021, 4, 08),
                DoseNumber = 2,
                LotNumber = "123456A",
                AdministeredAt = loc2,
                PatientId = patient1.Id,
                VaccineId = vaccine1.Id
            };
            context.Immunizations.Add(imm2);

            ImmunizationEntity imm3 = new ImmunizationEntity
            {
                AdministeredOnDate = new DateTime(2021, 1, 22),
                DoseNumber = 1,
                LotNumber = "MT0055",
                AdministeredAt = loc1,
                PatientId = patient2.Id,
                VaccineId = vaccine2.Id
            };
            context.Immunizations.Add(imm3);

            ImmunizationEntity imm4 = new ImmunizationEntity
            {
                AdministeredOnDate = new DateTime(2021, 2, 27),
                DoseNumber = 1,
                LotNumber = "AB1234",
                AdministeredAt = loc2,
                PatientId = patient3.Id,
                VaccineId = vaccine5.Id
            };
            context.Immunizations.Add(imm4);

            ImmunizationEntity imm5 = new ImmunizationEntity
            {
                AdministeredOnDate = new DateTime(2021, 4, 2),
                DoseNumber = 1,
                LotNumber = "AA3303",
                AdministeredAt = loc1,
                Patient = patient4,
                VaccineId = vaccine4.Id
            };
            context.Immunizations.Add(imm4);

            context.SaveChanges();
        }
    }
}