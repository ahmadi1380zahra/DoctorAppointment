using DoctorAppointment.Services.Patients.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Patients.Contracts
{
    public interface PatientService
    {
        Task Add(AddPatientDto dto);
        Task Update(int id,UpdatePatientDto dto);
        Task Delete(int id);
        List<GetPatientDto> GetAll();
    }
}
