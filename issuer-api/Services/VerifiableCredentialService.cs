using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using QRCoder;

using Issuer.Models;
using Issuer.HttpClients;
using Issuer.Models.Api;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Issuer.Services
{
    public class WebhookTopic
    {
        public const string Connections = "connections";
        public const string IssueCredential = "issue_credential";
        public const string RevocationRegistry = "revocation_registry";
        public const string BasicMessage = "basicmessages";
    }

    public class ConnectionState
    {
        public const string Invitation = "invitation";
        public const string Request = "request";
        public const string Response = "response";
        public const string Active = "active";
    }

    public class CredentialExchangeState
    {
        public const string OfferSent = "offer_sent";
        public const string RequestReceived = "request_received";
        public const string CredentialIssued = "credential_issued";
    }
    public class VerifiableCredentialService : BaseService, IVerifiableCredentialService
    {
        private readonly IVerifiableCredentialClient _verifiableCredentialClient;
        private readonly ILogger _logger;

        public VerifiableCredentialService(
            ApiDbContext context,
            IHttpContextAccessor httpContext,
            IVerifiableCredentialClient verifiableCredentialClient,
            IPatientService patientService,
            ILogger<VerifiableCredentialService> logger)
            : base(context, httpContext)
        {
            _verifiableCredentialClient = verifiableCredentialClient;
            _logger = logger;
        }

        // Handle webhook events pushed by the issuing agent.
        public async Task<bool> WebhookAsync(JObject data, string topic)
        {
            _logger.LogInformation("Webhook topic \"{topic}\"", topic);

            switch (topic)
            {
                case WebhookTopic.Connections:
                    return await HandleConnectionAsync(data);
                case WebhookTopic.IssueCredential:
                    return await HandleIssueCredentialAsync(data);
                case WebhookTopic.RevocationRegistry:
                    _logger.LogInformation("Revocation Registry data: for {@JObject}", JsonConvert.SerializeObject(data));
                    return true;
                case WebhookTopic.BasicMessage:
                    _logger.LogInformation("Basic Message data: for {@JObject}", JsonConvert.SerializeObject(data));
                    return false;
                default:
                    _logger.LogError("Webhook {topic} is not supported", topic);
                    return false;
            }
        }

        public async Task<int> IssueCredentialsAsync(Patient patient, List<Identifier> identifiers)
        {
            var connectionActive = true;
            var connection = await _context.Connections
                            .Where(c => c.AcceptedConnectionDate != null)
                            .OrderByDescending(c => c.AcceptedConnectionDate)
                            .FirstOrDefaultAsync(c => c.PatientId == patient.Id);

            if(connection == null)
            {
                // Create connection and wait for connection to be accepted before issuing credentials
                connection = await CreateConnectionAsync(patient);
                connectionActive = false;
            }

            var alias = patient.Id.ToString();
            var issuerDid = await _verifiableCredentialClient.GetIssuerDidAsync();
            var schemaId = await _verifiableCredentialClient.GetSchemaId(issuerDid);
            var credentialDefinitionId = await _verifiableCredentialClient.GetCredentialDefinitionIdAsync(schemaId);
            var credentials = new List<Credential>();

            foreach(var identifier in identifiers)
            {
                var newCredential = new Credential
                {
                    Connection = connection,
                    SchemaId = schemaId,
                    CredentialDefinitionId = credentialDefinitionId,
                    Identifier = identifier
                };

                credentials.Add(newCredential);
            }

            await _context.Credentials.AddRangeAsync(credentials);

            var created = await _context.SaveChangesAsync();

            if (created < 1)
            {
                throw new InvalidOperationException("Could not store credentials.");
            }

            if(connectionActive)
            {
                // Issue credentials if connection already active
                foreach(var credential in credentials)
                {
                    _logger.LogInformation("Issuing a credential with this connection_id: {connectionId}", connection.ConnectionId);
                    // Assumed that when a connection invitation has been sent and accepted
                    await IssueCredential(connection.ConnectionId, patient.Id, credential.Identifier.Uri);
                    _logger.LogInformation("Credential has been issued for connection_id: {connectionId}", connection.ConnectionId);
                }
            }

            return created;
        }

        // Create an invitation to establish a connection between the agents.
        private async Task<Connection> CreateConnectionAsync(Patient patient)
        {
            var connection = new Connection
            {
                PatientId = patient.Id
            };

            _context.Connections.Add(connection);

            var created = await _context.SaveChangesAsync();

            if (created < 1)
            {
                throw new InvalidOperationException("Could not store connection.");
            }

            await CreateInvitation(connection);

            return connection;
        }

        public async Task<bool> RevokeCredentialsAsync(int patientId)
        {
            var credentials = await _context.Credentials
                .Where(ec => ec.Connection.PatientId == patientId)
                .Where(ec => ec.CredentialExchangeId != null)
                .Where(ec => ec.RevokedCredentialDate == null)
                .ToListAsync();

            foreach (var credential in credentials)
            {
                var success = credential.AcceptedCredentialDate == null
                    ? await _verifiableCredentialClient.DeleteCredentialAsync(credential)
                    : await _verifiableCredentialClient.RevokeCredentialAsync(credential);

                if (success)
                {
                    credential.RevokedCredentialDate = DateTimeOffset.Now;
                    await _verifiableCredentialClient.SendMessageAsync(credential.Connection.ConnectionId, "This credential has been revoked.");
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<int> CreateInvitation(Connection connection)
        {
            var invitation = await _verifiableCredentialClient.CreateInvitationAsync(connection.PatientId.ToString());
            var invitationUrl = invitation.Value<string>("invitation_url");

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(invitationUrl, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);
            string qrCodeImageAsBase64 = qrCode.GetGraphic(20, "#003366", "#ffffff");

            connection.Base64QRCode = qrCodeImageAsBase64;
            return await _context.SaveChangesAsync();
        }

        // Handle webhook events for connection states.
        private async Task<bool> HandleConnectionAsync(JObject data)
        {
            var state = data.Value<string>("state");
            string connectionId;

            _logger.LogInformation("Connection state \"{state}\" for {@JObject}", state, JsonConvert.SerializeObject(data));

            switch (state)
            {
                case ConnectionState.Invitation:
                    // Patient Id stored as alias on invitation
                    await UpdateConnectionId(data.Value<int>("alias"), data.Value<string>("connection_id"));
                    return true;

                case ConnectionState.Request:
                    return true;

                case ConnectionState.Response:
                    var alias = data.Value<int>("alias");
                    connectionId = data.Value<string>("connection_id");

                    var connection = GetConnectionByConnectionIdAsync(connectionId);

                    var credentials = await _context.Credentials
                        .Where(c => c.ConnectionId == connection.Id)
                        .ToListAsync();

                    foreach(var credential in credentials)
                    {
                        _logger.LogInformation("Issuing a credential with this connection_id: {connectionId}", connectionId);
                        // Assumed that when a connection invitation has been sent and accepted
                        await IssueCredential(connectionId, alias, credential.Identifier.Uri);
                        _logger.LogInformation("Credential has been issued for connection_id: {connectionId}", connectionId);
                    }

                    return true;

                case ConnectionState.Active:
                    // Update connection accepted date
                    return true;

                default:
                    _logger.LogError("Connection state {state} is not supported", state);
                    return false;
            }
        }


        // Handle webhook events for issue credential topics.
        private async Task<bool> HandleIssueCredentialAsync(JObject data)
        {
            var state = data.Value<string>("state");

            _logger.LogInformation("Issue credential state \"{state}\" for {@JObject}", state, JsonConvert.SerializeObject(data));

            switch (state)
            {
                case CredentialExchangeState.OfferSent:
                case CredentialExchangeState.RequestReceived:
                    return true;
                case CredentialExchangeState.CredentialIssued:
                    await UpdateCredentialAfterIssued(data);
                    return true;
                default:
                    _logger.LogError("Credential exchange state {state} is not supported", state);
                    return false;
            }
        }

        private async Task<int> UpdateCredentialAfterIssued(JObject data)
        {
            var cred_ex_id = (string)data.SelectToken("cred_ex_id");

            var credential = GetCredentialByCredentialExchangeIdAsync(cred_ex_id);

            if (credential != null)
            {
                credential.AcceptedCredentialDate = DateTimeOffset.Now;
            }

            return await _context.SaveChangesAsync();
        }

        private async Task<int> UpdateConnectionId(int patientId, string connection_id)
        {
            // Add ConnectionId to Patient's newest connection
            var connection = await _context.Connections
                .Where(ec => ec.PatientId == patientId)
                .OrderByDescending(ec => ec.Id)
                .FirstOrDefaultAsync();

            if (connection != null)
            {
                _logger.LogInformation("Updating this connection's (Id = {id}) connectionId to {connection_id}", connection.Id, connection_id);

                connection.ConnectionId = connection_id;
                _context.Connections.Update(connection);
            }

            return await _context.SaveChangesAsync();
        }

        private Connection GetConnectionByConnectionIdAsync(string connectionId)
        {
            return _context.Connections
                    .SingleOrDefault(c => c.ConnectionId == connectionId);
        }

        private Credential GetCredentialByCredentialExchangeIdAsync(string credentialExchangeId)
        {
            return _context.Credentials
                    .SingleOrDefault(c => c.CredentialExchangeId == credentialExchangeId);
        }

        // Issue a credential to an active connection.
        private async Task<JObject> IssueCredential(string connectionId, int patientId, string url)
        {
            var patient = _context.Patients
                .SingleOrDefault(e => e.Id == patientId);

            var connection = GetConnectionByConnectionIdAsync(connectionId);

            // if (credential == null || credential.AcceptedCredentialDate != null)
            // {
            //     _logger.LogInformation("Cannot issue credential, credential with this connectionId:{connectionId} from database is null, or a credential has already been accepted.", connectionId);
            //     return null;
            // }

            var credentialAttributes = await CreateCredentialAttributesAsync(patientId, url);
            var credentialOffer = await CreateCredentialOfferAsync(connectionId, credentialAttributes);
            var issueCredentialResponse = await _verifiableCredentialClient.IssueCredentialAsync(credentialOffer);

            // // Set credentials CredentialExchangeId from issue credential response
            // credential.CredentialExchangeId = (string)issueCredentialResponse.SelectToken("credential_exchange_id");
            // _context.Credentials.Update(credential);

            // await _context.SaveChangesAsync();

            return issueCredentialResponse;
        }

        // Create the credential offer.
        private async Task<JObject> CreateCredentialOfferAsync(string connectionId, JArray attributes)
        {
            var issuerDid = await _verifiableCredentialClient.GetIssuerDidAsync();
            var schemaId = await _verifiableCredentialClient.GetSchemaId(issuerDid);
            var schema = (await _verifiableCredentialClient.GetSchema(schemaId)).Value<JObject>("schema");
            var credentialDefinitionId = await _verifiableCredentialClient.GetCredentialDefinitionIdAsync(schemaId);

            JObject credentialOffer = new JObject
            {
                { "connection_id", connectionId },
                { "issuer_did", issuerDid },
                { "schema_id", schemaId },
                { "schema_issuer_did", issuerDid },
                { "schema_name", schema.Value<string>("name") },
                { "schema_version", schema.Value<string>("version") },
                { "cred_def_id", credentialDefinitionId },
                { "comment", "PharmaNet GPID" },
                { "auto_remove", false },
                { "trace", false },
                {
                    "credential_proposal",
                    new JObject
                        {
                            { "@type", "did:sov:BzCbsNYhMrjHiqZDTUASHg;spec/issue-credential/1.0/credential-preview" },
                            { "attributes", attributes }
                        }
                }
            };

            _logger.LogInformation("Credential offer for connection ID \"{connectionId}\" for {@JObject}", connectionId, JsonConvert.SerializeObject(credentialOffer));

            return credentialOffer;
        }

        // Create the credential proposal attributes.
        private async Task<JArray> CreateCredentialAttributesAsync(int patientId, string url)
        {
            // var record = _immunizationClient
            var immunizationRecord = new ImmunizationRecordResponse();

            var attributes = new JArray
            {
                new JObject
                {
                    { "name", "test"},
                    { "value", "test" }
                }
            };

            // var attributes = new JArray
            // {
            //     new JObject
            //     {
            //         { "name", "Lot Number"},
            //         { "value", immunizationRecord.LotNumber }
            //     },
            //     new JObject
            //     {
            //         { "name", "Date of Vaccination" },
            //         { "value", immunizationRecord.VaccinationDate }
            //     },
            //     new JObject
            //     {
            //         { "name", "Dose Number" },
            //         { "value", immunizationRecord.DoseNumber }
            //     },
            //     new JObject
            //     {
            //         { "name", "Country of Vaccination" },
            //         { "value", immunizationRecord.CountryOfVaccination }
            //     },
            //     new JObject
            //     {
            //         { "name", "Administering Centre" },
            //         { "value", immunizationRecord.Facility }
            //     },
            //     new JObject
            //     {
            //         { "name", "Next Vaccination Date" },
            //         { "value", immunizationRecord.NextVaccinationDueDate }
            //     }
            // };

            _logger.LogInformation("Credential offer attributes for {@JObject}", JsonConvert.SerializeObject(attributes));

            return attributes;
        }
    }
}
