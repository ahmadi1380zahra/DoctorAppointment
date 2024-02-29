using DoctorAppointment.Services.Patients.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools.Patients
{
    public static class AddPatientDtoFactory
    {
        public static AddPatientDto Create(string? nationalCode=null)
        {
            return new AddPatientDto()
            {
                FirstName = "zahra",
                LastName = "Ahmadi",
                NationalCode = nationalCode ?? "7676"
            };
        }
    }
}
