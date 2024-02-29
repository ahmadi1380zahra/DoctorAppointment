using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Contracts;
using DoctorAppointment.Services.Patients.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Persistance.EF.Patients
{
    public class EFPatientRepository : PatientRepository
    {
        private readonly EFDataContext _context;
        public EFPatientRepository(EFDataContext context)
        {
            _context = context;
        }

        public void Add(Patient patient)
        {
            _context.Patients.Add(patient);
        }

        public void Delete(Patient patient)
        {
            _context.Patients.Remove(patient);
        }

        public Patient? Find(int id)
        {
            return _context.Patients.FirstOrDefault(_ => _.Id == id);

        }

        public List<GetPatientDto> GetAll()
        {
          IQueryable<Patient> query= _context.Patients;
            List<GetPatientDto> patients = query.Select(patient => new GetPatientDto
            {
                Id=patient.Id,
                FirstName=patient.FirstName,
                LastName=patient.LastName,
                NationalCode=patient.NationalCode,
            }).ToList();
            return patients;
        }

        public bool IsExistNationalCode(string nationalCode)
        {
           return _context.Patients.Any(_=>_.NationalCode == nationalCode);
        }

        public bool IsExistNationalCodeExceptItSelf(int id, string nationalCode)
        {
            return _context.Patients.Any(_=>_.NationalCode==nationalCode && _.Id!=id);
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
        }
    }
}
