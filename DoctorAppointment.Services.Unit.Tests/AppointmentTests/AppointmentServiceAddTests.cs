using DoctorAppointment.Entities.Appointments;
using DoctorAppointment.Entities.Doctors;
using DoctorAppointment.Entities.Patients;
using DoctorAppointment.Persistance.EF;
using DoctorAppointment.Persistance.EF.Appointments;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Appointments;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Appointments.Contracts.Dtos;
using DoctorAppointment.Services.Appointments.Exceptions;
using DoctorAppointment.Services.Doctors.Exceptions;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Exceptions;
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
    public class AppointmentServiceAddTests
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly AppointmentService _sut;
        public AppointmentServiceAddTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = AppointmentServiceFactory.Create(_context);
        }
        [Fact]
        public async Task Add_adds_a_new_appointment_properly()
        {
            var doctor = new DoctorBuilder().Build();
            var patient = new PatientBuilder().Build();
            _context.Save(patient);
            _context.Save(doctor);
            var dto = AddAppointmentDtoFactory.Create(doctor.Id, patient.Id);

            await _sut.Add(dto);

            var actual = _readContext.Appointments.Single();
            actual.PatientId.Should().Be(dto.PatientId);
            actual.DoctorId.Should().Be(dto.DoctorId);
            actual.Date.Should().Be(dto.Date);
        }
        [Fact]
        public async Task Add_throw_DoctorIsNotExistedException()
        {
            var patient = new PatientBuilder().Build();
            _context.Save(patient);
            var dummyDoctorId = 12;
            var dto = AddAppointmentDtoFactory.Create(patient.Id, dummyDoctorId);

            var actual = () => _sut.Add(dto);

            await actual.Should().ThrowExactlyAsync<DoctorIsNotExistedException>();
        }
        [Fact]
        public async Task Add_throw_PatientIsNotExistedException()
        {
            var doctor = new DoctorBuilder().Build();
            _context.Save(doctor);
            var dummyPatientId = 12;
            var dto = AddAppointmentDtoFactory.Create(doctor.Id, dummyPatientId);

            var actual = () => _sut.Add(dto);

            await actual.Should().ThrowExactlyAsync<PatientIsNotExistedException>();
        }
        [Fact]
        public async Task Add_throw_DoctorCantHaveMoreThanFiveAppointmentPerDayException()
        {
            var doctor = new DoctorBuilder().Build();
            _context.Save(doctor);
            var patient = new PatientBuilder().Build();
            _context.Save(patient);
            var dto = AddAppointmentDtoFactory.Create(doctor.Id, patient.Id);
            await _sut.Add(dto);
            await _sut.Add(dto);
            await _sut.Add(dto);
            await _sut.Add(dto);
            await _sut.Add(dto);

            var actual = () => _sut.Add(dto);

            await actual.Should().ThrowExactlyAsync<DoctorCantHaveMoreThanFiveAppointmentPerDayException>();
       }
    }
}
