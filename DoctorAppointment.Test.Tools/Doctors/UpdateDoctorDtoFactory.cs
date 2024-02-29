using DoctorAppointment.Services.Doctors.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools.Doctors
{
    public static class UpdateDoctorDtoFactory
    {
        public static UpdateDoctorDto Create(string? nationalCode=null)
        {
            return new UpdateDoctorDto
            {
                FirstName = "dummy-first-name",
                LastName = "dummy-last-name",
                Field = "heart",
                NationalCode = nationalCode?? "dummy-national-code"
            };
        }
    }
}
