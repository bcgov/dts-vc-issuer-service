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
namespace ImmunizationApi.Models.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using System.Collections.Generic;

    /// <summary>
    /// Represents basic Patient or Subject Information
    /// </summary>
    public class PatientEntity
    {
        /// <summary>
        /// Gets or sets the subject's Identifier.
        /// </summary>j
        [Key]
        public string Id { get; set; } 

        /// <summary>
        /// Gets or sets the Subject's LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Subject's Given Name(s)
        /// </summary>
        public string GivenNames { get; set; }

        /// <summary>
        /// Gets or sets the subject's birthdate.
        /// </summary>
        public DateTime BirthDate { get; set; }

    }
}
