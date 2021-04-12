//-------------------------------------------------------------------------
// Copyright © 2019 Province of British Columbia
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-------------------------------------------------------------------------
namespace ImmunizationApi.Repository.Example
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using ImmunizationApi.Models;
    using ImmunizationApi.Models.Entity;
    using ImmunizationApi.Repository;

    /// <summary>
    /// The Immunization Example data service.
    /// </summary>
    public class ImmunizationRepository : IImmunizationRepository
    {
        private readonly ImmunizationDbContext dbContext;
        private readonly ILogger<ImmunizationRepository> logger;

        public ImmunizationRepository(ImmunizationDbContext dbContext, ILogger<ImmunizationRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        private Immunization FromEntity(ImmunizationEntity imz)
        {
            Immunization immunization = null;
            if (imz != null)
            {
                immunization = new Immunization
                {
                    RecordIdentifier = imz.Id.ToString(),
                    VaccinationDate = imz.AdministeredOnDate.ToShortDateString(),
                    NextVaccinationDueDate = imz.NextDueDate.ToShortDateString(),
                    DoseNumber = imz.DoseNumber,
                    LotNumber = imz.LotNumber,
                    CountryOfVaccination = imz.AdministeredAt.Country,
                    Facility = imz.AdministeredAt.Name,

                    Patient = new Patient
                    {
                        Identifier = new Identifier
                        {
                            Value = imz.Patient.Id,
                            SystemUri = $"urn:oid:2.16.840.1.113883.4.50",  // BC PHN HL7 Object Identifier IRI.
                        },
                        BirthDate = imz.Patient.BirthDate.ToShortDateString(),
                        FullName = imz.Patient.LastName.ToUpper() + ", " + imz.Patient.GivenNames,
                        ReferenceURI = "urn:/Patient/" + imz.PatientId

                    },
                    Vaccine = new Vaccine
                    {
                        VaccineIdentifier = new Identifier
                        {
                            Value = imz.Vaccine.Id,
                            SystemUri = $"http://hl7.org/fhir/NamingSystem/ca-hc-din"  // Canada Drug Identification Number
                        },
                        Manufacturer = imz.Vaccine.Manufacturer,
                        ProductName = imz.Vaccine.Name,
                        Disease = imz.Vaccine.Disease
                    }

                };
            }
            return immunization;
        }
        public async Task<IEnumerable<Immunization>> Find(string patientId)
        {
            List<Immunization> immunizations = new List<Immunization>();

            var records = await dbContext.Immunizations
                .Where(i => i.PatientId == patientId)
                .Include(i => i.Patient)
                .Include(i => i.Vaccine)
                .Include(i => i.AdministeredAt).ToListAsync<ImmunizationEntity>();

            foreach (ImmunizationEntity imz in records)
            {
                Immunization i = this.FromEntity(imz);
                immunizations.Add(i);
            }
            return immunizations;

        }

        public async Task<Immunization> Get(string immunizationId)
        {
            Immunization immunization = null;

            ImmunizationEntity imz = await dbContext.Immunizations
                .Where(i => i.Id.ToString() == immunizationId)
                .Include(i => i.Patient)
                .Include(i => i.Vaccine)
                .Include(i => i.AdministeredAt).FirstAsync<ImmunizationEntity>();
            
            immunization = this.FromEntity(imz);
            return immunization;
        }
    }
}

