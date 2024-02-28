using DoctorAppointment.Services.Patients.Contracts.Dtos;
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
        void Delete(Patient patient);
        Patient Find(int id);
        List<GetPatientDto> GetAll();
        bool IsExistNationalCode(string nationalCode);
        bool IsExistNationalCodeExceptItSelf(int id, string nationalCode);
        void Update(Patient? patient);
    }
}
