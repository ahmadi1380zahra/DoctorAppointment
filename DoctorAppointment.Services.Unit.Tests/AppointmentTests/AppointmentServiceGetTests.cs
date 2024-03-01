using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Test.Tools.Appointments;
using DoctorAppointment.Test.Tools.Doctors;
using DoctorAppointment.Test.Tools.Infrastructure.DatabaseConfig.Unit;
using DoctorAppointment.Test.Tools.Patients;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Unit.Tests.AppointmentTests
{
    public class AppointmentServiceGetTests
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly AppointmentService _sut;
        public AppointmentServiceGetTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = AppointmentServiceFactory.Create(_context);
        }
        [Fact]
        public async Task GetAll_gets_all_appointments_count()
        {
            var patient = new PatientBuilder().Build();
            _context.Save(patient);
            var doctor = new DoctorBuilder().Build();
            _context.Save(doctor);
            var appointment = new AppointmentBuilder(patient.Id, doctor.Id).Build();
            var appointment2 = new AppointmentBuilder(patient.Id, doctor.Id).Build();
            var appointment3 = new AppointmentBuilder(patient.Id, doctor.Id).Build();
            _context.Save(appointment);
            _context.Save(appointment2);
            _context.Save(appointment3);

            var actual = await _sut.GetAll();

            actual.Count().Should().Be(3);
        }
        [Fact]
        public async Task GetAll_gets_an_appointment_to_check_for_valid_data()
        {
            var patient = new PatientBuilder().Build();
            _context.Save(patient);
            var doctor = new DoctorBuilder().Build();
            _context.Save(doctor);
            var appointment = new AppointmentBuilder(patient.Id, doctor.Id).Build();
            _context.Save(appointment);

            var appointments = await _sut.GetAll();

            var actual = appointments.FirstOrDefault();
            actual.Date.Should().Be(appointment.Date);
            actual.PatientName.Should().Be(appointment.Patient.FirstName + appointment.Patient.LastName);
            actual.DoctorName.Should().Be(appointment.Doctor.FirstName + appointment.Doctor.LastName);
        }
    }
}
