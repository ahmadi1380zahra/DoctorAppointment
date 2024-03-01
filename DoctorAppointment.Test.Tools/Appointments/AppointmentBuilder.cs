using DoctorAppointment.Entities.Appointments;
using DoctorAppointment.Entities.Doctors;
using DoctorAppointment.Entities.Patients;
using DoctorAppointment.Test.Tools.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools.Appointments
{
    public class AppointmentBuilder
    {
        private readonly Appointment _appointment;
        public AppointmentBuilder(int patientId, int doctorId)
        {
            _appointment = new Appointment
            {
                Date = new DateTime(2024 / 01 / 03),
                PatientId = patientId,
                DoctorId = doctorId,
            };
        }
        public AppointmentBuilder WithDate(DateTime date)
        {
            _appointment.Date = date;
            return this;
        }
        public Appointment Build()
        {
            return _appointment;
        }
    }
}
