//-------------------------------------------------------------------------
// Copyright © 2021 Province of British Columbia
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
namespace ImmunizationApi.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents an Immunization Record.
    /// </summary>
    public class Immunization
    {
        /// <summary>
        /// Gets or sets this immunization Fully qualified URL
        /// </summary>
        [JsonPropertyName("fullUrl")]
        public string FullUrl { get; set; }

        /// <summary>
        /// Gets or sets the immunization record identifier
        /// </summary>
        [JsonPropertyName("id")]
        public string RecordIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the Vaccine Description,a generic description and codification of the 
        /// </summary>
        [JsonPropertyName("patient")]
        public Patient Patient { get; set; } = new Patient();

        /// <summary>
        /// Gets or sets the Vaccine Description,a generic description and codification of the 
        /// </summary>
        [JsonPropertyName("vaccine")]
        public Vaccine Vaccine { get; set; } = new Vaccine();

        /// <summary>
        /// Gets or sets the lot Number (aka Batch Number)
        /// </summary>
        [JsonPropertyName("lotNumber")]
        public string LotNumber { get; set; }

        /// <summary>
        /// Gets or sets the date of vaccination: ISO 8601
        /// </summary>
        [JsonPropertyName("dateOfVaccination")]
        public string VaccinationDate { get; set; }

        /// <summary>
        /// Gets or sets the date of vaccination: ISO 8601
        /// </summary>
        [JsonPropertyName("doseNumber")]
        public int DoseNumber { get; set; }

        /// <summary>
        /// Gets or sets the country of vaccination: ISO 3166
        /// </summary>
        [JsonPropertyName("countryOfVaccination")]
        public string CountryOfVaccination { get; set; }

        /// <summary>
        /// Gets or sets the name of the vaccination facility
        /// </summary>
        [JsonPropertyName("administeringCentre")]
        public string Facility { get; set; }

        /// <summary>
        /// Gets or sets the name of the next Vaccination due-by Date.
        /// ISO 8601
        /// </summary>
        [JsonPropertyName("nextVaccinationDate")]
        public string NextVaccinationDueDate { get; set; }

    }
}
