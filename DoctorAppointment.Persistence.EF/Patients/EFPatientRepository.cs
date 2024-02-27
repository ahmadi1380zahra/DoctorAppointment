using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Contracts;
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

        public Patient? Find(int id)
        {
            return _context.Patients.Find(id);
        }

        public bool IsExistNationalCode(string nationalCode)
        {
           return _context.Patients.Any(_=>_.NationalCode == nationalCode);
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
        }
    }
}
