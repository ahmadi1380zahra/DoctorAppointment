using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Appointments.Exceptions;
using DoctorAppointment.Test.Tools.Appointments;
using DoctorAppointment.Test.Tools.Doctors;
using DoctorAppointment.Test.Tools.Infrastructure.DatabaseConfig.Unit;
using DoctorAppointment.Test.Tools.Patients;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Unit.Tests.AppointmentTests
{
    public class AppointmentServiceDeleteTests
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly AppointmentService _sut;
        public AppointmentServiceDeleteTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = AppointmentServiceFactory.Create(_context);
        }

        [Fact]
        public async Task Delete_deletes_an_appointment_properly()
        {
            var patient = new PatientBuilder().Build();
            _context.Save(patient);
            var doctor = new DoctorBuilder().Build();
            _context.Save(doctor);
            var appointment = new AppointmentBuilder(patient.Id, doctor.Id).Build();
            _context.Save(appointment);

            await _sut.Delete(appointment.Id);

            var actual = _readContext.Appointments.FirstOrDefaultAsync(_ => _.Id == appointment.Id);
            actual.Should().NotBeNull();
        }
        [Fact]
        public async Task Delete_throws_AppointmentIsNotExistException()
        {
            var dummyId = 11;

            var actual = () => _sut.Delete(dummyId);

            await actual.Should().ThrowExactlyAsync<AppointmentIsNotExistException>();
        }
    }
}
