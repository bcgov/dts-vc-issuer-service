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
namespace ImmunizationApi.Services.Example
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImmunizationApi.Models;

    using ImmunizationApi.Repository;

    /// <summary>
    /// The Immunization data service.
    /// </summary>
    public class ImmunizationService : IImmunizationService
    {
        private readonly IImmunizationRepository repository;

        public ImmunizationService(IImmunizationRepository repo)
        {
            this.repository = repo;
        }

        /// <inherited/>
        public Task<IEnumerable<Immunization>> GetImmunizations(string patientId, string accessToken = null)
        {
            Task<IEnumerable<Immunization>> immunizations = repository.Find(patientId);
            return immunizations;
        }

        /// <inherited/>
        public Task<Immunization> GetImmunization(string immunizationId, string accessToken = null)
        {
            Task<Immunization> immunization =  repository.Get(immunizationId);
            return immunization;
        }
    }
}
