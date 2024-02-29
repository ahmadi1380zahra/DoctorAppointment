using DoctorAppointment.Services.Doctors.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools.Doctors
{
    public static class AddDoctorDtoFactory
    {
        public static AddDoctorDto Create(string? nationalCode = null)
        {
            return new AddDoctorDto
            {
                FirstName = "dummy-first-name",
                LastName = "dummy-last-name",
                Field = "heart",
                NationalCode = nationalCode ?? "2283182033"
            };
        }
    }
}
