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
namespace ImmunizationApi.Controllers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using ImmunizationApi.Models;
    using ImmunizationApi.Services;

    /// <summary>
    /// The Immunization controller.
    /// </summary>
    //[Authorize]
    [Route("/api/[controller]")]
    [ApiController]
    public class ImmunizationController : ControllerBase
    {
        private readonly ILogger logger;

        /// <summary>
        /// Gets or sets the immunization data service.
        /// </summary>
        private readonly IImmunizationService service;

        /// <summary>
        /// Gets or sets the http context accessor.
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        private string ResourceBaseUrl()
        {
            HostString host = this.httpContextAccessor.HttpContext.Request.Host;
            string uriProto = this.httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://";
            return uriProto + host.ToUriComponent() + this.httpContextAccessor.HttpContext.Request.Path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmunizationController"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="svc">The immunization data service.</param>
        /// <param name="httpContextAccessor">The Http Context accessor.</param>
        public ImmunizationController(ILogger<ImmunizationController> logger,
            IImmunizationService service,
            IHttpContextAccessor httpContextAccessor)

        {
            this.logger = logger;
            this.service = service;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("{immunizationId}")] // GET Matches api/Immunization/{id}
        [Produces("application/json")]
        //[Authorize]
        public async Task<IActionResult> GetImmunization(string immunizationId)
        {
            try
            {
                Immunization immunization = await service.GetImmunization(immunizationId);
                if (immunization == null)
                    return NotFound();
                immunization.FullUrl = this.ResourceBaseUrl();
                return new JsonResult(immunization);
            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex.ToString());
                return NotFound();
            }

        }

        [HttpGet] // GET  api/Immunization?patient=39393993
        [Produces("application/json")]
        //[Authorize]
        public async Task<IActionResult> GetImmunizations(string patientId)
        {
            try
            {
                IEnumerable<Immunization> immunizations = await service.GetImmunizations(patientId);

                if (Enumerable.Count<Immunization>(immunizations) == 0)
                    return NotFound();

                foreach (Immunization i in immunizations)
                {
                    i.FullUrl = this.ResourceBaseUrl() + "/" + i.RecordIdentifier;
                }
                return new JsonResult(immunizations);

            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex.ToString());
                return NotFound();
            }
        }
    }
}
