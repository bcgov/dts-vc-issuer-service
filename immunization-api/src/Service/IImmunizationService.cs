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
namespace ImmunizationApi.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImmunizationApi.Models;

    /// <summary>
    /// The Immunization data service.
    /// </summary>
    public interface IImmunizationService
    {
        /// <summary>
        /// Gets the Immunization including load state and a list of immunization records.
        /// </summary>
        /// <param name="patientId">A unique patient identifier.</param>
        /// <param name="accessToken">The authorization access token representing the authenticated user (optional).</param>
        /// <returns>Returns a list of immunizations.</returns>
        public Task<IEnumerable<Immunization>> GetImmunizations(string patientId, string accessToken = null);


        /// <summary>
        /// Gets the ImmunizationResult including load state and a list of immunization records.
        /// </summary>
        /// <param name="immunizationId">A unique immunization record identifier.</param>
        /// <param name="bearerToken">The security token representing the authenticated user.</param>
        /// <returns>Returns a list of immunizations.</returns>
        public Task<Immunization> GetImmunization(string immunizationId, string accessToken = null);


    }
}
