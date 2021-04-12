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
    /// Represents basic Patient or Subject Information
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Gets or sets the Subject's Full name "SURNAME GIVEN-NAME(S)"
        /// </summary>
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the subject's birthdate.  ISO 9601
        /// </summary>
        [JsonPropertyName("birthDate")]
        public string BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the subject's Identifier.
        /// </summary>j
        [JsonPropertyName("identifier")]
        public Identifier Identifier { get; set; } = new Identifier();

        /// <summary>
        /// Gets or sets the subject's unique URI for this patient.
        /// </summary>j
        [JsonPropertyName("reference")]
        public string ReferenceURI { get; set; }
    }
}
