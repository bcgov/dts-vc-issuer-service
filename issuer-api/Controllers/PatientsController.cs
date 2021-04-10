using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Issuer.Models;
using Issuer.Models.Api;
using Issuer.Services;

namespace Issuer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IVerifiableCredentialService _verifiableCredentialService;

        public PatientsController(
            IPatientService patientService,
            IVerifiableCredentialService verifiableCredentialService)
        {
            _patientService = patientService;
            _verifiableCredentialService = verifiableCredentialService;
        }

        // GET: api/Patients/026a6bf5-3a7a-4b63-8dd6-f9a10ce30bbb
        /// <summary>
        /// Gets Patient by UserId
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("{userId}:Guid", Name = nameof(GetPatientByUserId))]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResultResponse<Patient>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPatientByUserId(Guid userId)
        {
            if(userId != User.GetissuerUserId())
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

            if (await _patientService.UserIdExistsAsync(User.GetissuerUserId()))
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

        // POST: api/Patients/5/credential
        /// <summary>
        /// Issues Credentials for a patient.
        /// Creates a new connection if no active connection exists.
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="identifiers"></param>
        [HttpPost("{patientId}/credential", Name = nameof(Credential))]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResultResponse<Credential>), StatusCodes.Status201Created)]
        public async Task<ActionResult> Credential(int patientId, List<Identifier> identifiers)
        {
            var patient = await _patientService.GetPatientAsync(patientId);
            if(patient == null)
            {
                return BadRequest();
            }

            await _verifiableCredentialService.IssueCredentialsAsync(patient, identifiers);

            return Ok();
        }

    }
}
