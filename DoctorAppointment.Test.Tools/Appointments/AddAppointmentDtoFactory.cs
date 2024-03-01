using DoctorAppointment.Entities.Doctors;
using DoctorAppointment.Entities.Patients;
using DoctorAppointment.Services.Appointments.Contracts.Dtos;
using DoctorAppointment.Test.Tools.Doctors;
using DoctorAppointment.Test.Tools.Patients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools.Appointments
{
    public static class AddAppointmentDtoFactory
    {
        public static AddAppointmentDto Create(int doctorId, int patientId,DateTime? date=null)
        {
            return new AddAppointmentDto
            {
                DoctorId = doctorId,
                PatientId = patientId,
                Date =date?? new DateTime(2024, 02, 02)
            };
        }
    }
}
