using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Prime.Models;
using Prime.Models.Api;
using Prime.Services;

namespace Prime.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(
            IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/Patients/026a6bf5-3a7a-4b63-8dd6-f9a10ce30bbb
        /// <summary>
        /// Gets Patient by UserId
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("{userId}", Name = nameof(GetPatientByUserId))]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResultResponse<Patient>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPatientByUserId(Guid userId)
        {
            if(userId != User.GetPrimeUserId())
            {
                return Forbid();
            }
            var patient = await _patientService.GetPatientForUserIdAsync(userId);
            return Ok(ApiResponse.Result( patient ));
        }

        // GET: api/Patients/5
        /// <summary>
        /// Gets a specific Patient.
        /// </summary>
        /// <param name="patientId"></param>
        [HttpGet("{patientId}", Name = nameof(GetPatientById))]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiMessageResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResultResponse<Patient>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPatientById(int patientId)
        {
            var patient = await _patientService.GetPatientAsync(patientId);
            if (patient == null)
            {
                return NotFound(ApiResponse.Message($"Patient not found with id {patientId}"));
            }

            return Ok(ApiResponse.Result(patient));
        }

        // POST: api/Patients
        /// <summary>
        /// Creates a new Patient.
        /// </summary>
        [HttpPost(Name = nameof(CreatePatient))]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResultResponse<Patient>), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreatePatient(Patient patient)
        {
            if (patient == null)
            {
                ModelState.AddModelError("Patient", "Could not create an patient, the passed in Patient cannot be null.");
                return BadRequest(ApiResponse.BadRequest(ModelState));
            }

            if (await _patientService.UserIdExistsAsync(User.GetPrimeUserId()))
            {
                ModelState.AddModelError("Patient.UserId", "An patient already exists for this User Id, only one patient is allowed per User Id.");
                return BadRequest(ApiResponse.BadRequest(ModelState));
            }

            var createdPatientId = await _patientService.CreatePatientAsync(patient);
            patient = await _patientService.GetPatientAsync(createdPatientId);

            return CreatedAtAction(
                nameof(GetPatientById),
                new { patientId = createdPatientId },
                ApiResponse.Result(patient)
            );
        }

    }
}
