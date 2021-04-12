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
namespace ImmunizationApi.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ImmunizationApi.Models;


    /// <summary>
    /// The Immunization data service.
    /// </summary>5
    public interface IImmunizationRepository
    {
        /// <summary>
        /// Gets the ImmunizationResult including load state and a list of immunization records.
        /// </summary>
        /// <param name="patientId">The patient identifier user.</param>
        /// <returns>Returns a list of immunizations.</returns>
        Task<IEnumerable<Immunization>> Find(string patientId);

                /// <summary>
        /// Gets the ImmunizationResult including load state and a list of immunization records.
        /// </summary>
        /// <returns>Returns a list of immunizations.</returns>
        Task<Immunization> Get(string immunizationId);
    }
}