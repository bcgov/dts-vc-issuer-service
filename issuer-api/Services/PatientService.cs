using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Prime.Models;

namespace Prime.Services
{
    public class PatientService : BaseService, IPatientService
    {
        public PatientService(
            ApiDbContext context,
            IHttpContextAccessor httpContext)
            : base(context, httpContext)
        {
        }

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _context.Patients
                .AsNoTracking()
                .AnyAsync(e => e.Id == patientId);
        }

        public async Task<bool> UserIdExistsAsync(Guid userId)
        {
            return await _context.Patients
                .AsNoTracking()
                .AnyAsync(e => e.UserId == userId);
        }

        public async Task<Patient> GetPatientForUserIdAsync(Guid userId)
        {
            return await _context.Patients
                .Include(e => e.PatientCredentials)
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task<Patient> GetPatientAsync(int patientId)
        {
            return await _context.Patients
                .Include(e => e.PatientCredentials)
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == patientId);
        }

        public async Task<int> CreatePatientAsync(Patient newPatient)
        {
            newPatient.ThrowIfNull(nameof(newPatient));

            _context.Patients.Add(newPatient);

            var created = await _context.SaveChangesAsync();
            if (created < 1)
            {
                throw new InvalidOperationException("Could not create patient.");
            }

            return newPatient.Id;
        }
    }
}
