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
    /// Represents basic Vaccine Information
    /// </summary>
    public class Vaccine
    {
        /// <summary>
        /// Gets or the Identifier for the drug (e.g. DIN code)
        /// </summary>
        [JsonPropertyName("identifier")]
        public Identifier VaccineIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the vaccine manufacturer
        /// </summary>
        [JsonPropertyName("manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the vaccine manufacturer
        /// </summary>
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the vaccine's targeted disease
        /// </summary>
        [JsonPropertyName("disease")]
        public string Disease { get; set; }
    }
}
