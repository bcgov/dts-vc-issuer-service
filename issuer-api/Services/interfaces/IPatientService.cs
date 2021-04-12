using System;
using System.Threading.Tasks;
using Issuer.Models;

namespace Issuer.Services
{
    public interface IPatientService
    {
        Task<Patient> GetPatientForUserIdAsync(Guid userId);
        Task<Patient> GetPatientAsync(int patientId);
        Task<bool> PatientExistsAsync(int patientId);
        Task<bool> UserIdExistsAsync(Guid userId);
        Task<int> CreatePatientAsync(Patient patient);
    }
}
