using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Patients.Contracts
{
    public interface PatientRepository
    {
        void Add(Patient patient);
        Patient Find(int id);
        bool IsExistNationalCode(string nationalCode);
        void Update(Patient? patient);
    }
}
