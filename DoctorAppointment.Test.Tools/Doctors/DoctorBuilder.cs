using DoctorAppointment.Entities.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools.Doctors
{
    public class DoctorBuilder
    {
        private readonly Doctor _doctor;
        public DoctorBuilder()
        {
            _doctor = new Doctor
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                Field = "heart",
                NationalCode = "12345673"
            };
        }
        public DoctorBuilder WithNationalCode(string nationalCode)
        {
            _doctor.NationalCode = nationalCode;
            return this;
        }
        public Doctor Build()
        {
            return _doctor;
        }
    }
}
