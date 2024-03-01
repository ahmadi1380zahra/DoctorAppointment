using DoctorAppointment.Entities.Appointments;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Appointments.Contracts.Dtos;
using DoctorAppointment.Services.Appointments.Exceptions;
using DoctorAppointment.Services.Patients.Exceptions;
using DoctorAppointment.Test.Tools.Appointments;
using DoctorAppointment.Test.Tools.Doctors;
using DoctorAppointment.Test.Tools.Infrastructure.DatabaseConfig.Unit;
using DoctorAppointment.Test.Tools.Patients;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Unit.Tests.AppointmentTests
{
    public class AppointmentServiceUpdateTests
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly AppointmentService _sut;
        public AppointmentServiceUpdateTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = AppointmentServiceFactory.Create(_context);
        }
        [Fact]
        public async Task Update_updates_an_appointment_properly()
        {
            var doctor = new DoctorBuilder().Build();
            var patient = new PatientBuilder().Build();
            _context.Save(patient);
            _context.Save(doctor);
            var appointment = new AppointmentBuilder(patient.Id, doctor.Id).Build();
            _context.Save(appointment);
            var updateDoctor = new DoctorBuilder().Build();
            var updatePatient = new PatientBuilder().Build();
            _context.Save(updateDoctor);
            _context.Save(updatePatient);
            var updateDto = UpdateAppointmentDtoFactory.Create(updateDoctor.Id, updatePatient.Id);

            await _sut.Update(appointment.Id, updateDto);

            var actual = _readContext.Appointments.Single();
            actual.PatientId.Should().Be(updateDto.PatientId);
            actual.DoctorId.Should().Be(updateDto.DoctorId);
            actual.Date.Should().Be(updateDto.Date);
        }
        [Fact]
        public async Task Update_throws_AppointmentIsNotExistToUpdateException()
        {
            var patient = new PatientBuilder().Build();
            var doctor = new DoctorBuilder().Build();
            _context.Save(patient);
            _context.Save(doctor);
            var updateDto = UpdateAppointmentDtoFactory.Create(doctor.Id, patient.Id);
            var dummyId = 11;

            var actual = () => _sut.Update(dummyId, updateDto);

            await actual.Should().ThrowExactlyAsync<AppointmentIsNotExistToUpdateException>();
        }
        [Fact]
        public async Task Update_throws_DoctorIsNotExistedException()
        {
            var patient = new PatientBuilder().Build();
            _context.Save(patient);
            var doctor = new DoctorBuilder().Build();
            _context.Save(doctor);
            var appointment = new AppointmentBuilder(patient.Id, doctor.Id).Build();
            _context.Save(appointment);
            var doctorDummyId = 11;
            var updateDto = UpdateAppointmentDtoFactory.Create(doctorDummyId, patient.Id);

            var actual = () => _sut.Update(appointment.Id, updateDto);

            await actual.Should().ThrowExactlyAsync<DoctorIsNotExistedException>();
        }
        [Fact]
        public async Task Update_throws_PatientIsNotExistedException()
        {
            var patient = new PatientBuilder().Build();
            _context.Save(patient);
            var doctor = new DoctorBuilder().Build();
            _context.Save(doctor);
            var appointment = new AppointmentBuilder(patient.Id, doctor.Id).Build();
            _context.Save(appointment);
            var patientDummyId = 11;
            var updateDto = UpdateAppointmentDtoFactory.Create(doctor.Id, patientDummyId);

            var actual = () => _sut.Update(appointment.Id, updateDto);

            await actual.Should().ThrowAsync<PatientIsNotExistedException>();
        }
        [Fact]
        public async Task Update_throws_DoctorCantHaveMoreThanFiveAppointmentPerDayException()
        {
            var date= DateTime.Now;
            var patient = new PatientBuilder().Build();
            _context.Save(patient);
            var doctor = new DoctorBuilder().Build();
            _context.Save(doctor);
            var appointment1 = new AppointmentBuilder(patient.Id, doctor.Id).WithDate(date).Build();
            var appointment2 = new AppointmentBuilder(patient.Id, doctor.Id).WithDate(date).Build();
            var appointment3 = new AppointmentBuilder(patient.Id, doctor.Id).WithDate(date).Build();
            var appointment4 = new AppointmentBuilder(patient.Id, doctor.Id).WithDate(date).Build();
            var appointment5 = new AppointmentBuilder(patient.Id, doctor.Id).WithDate(date).Build();
            _context.Save(appointment1);
            _context.Save(appointment2);
            _context.Save(appointment3);
            _context.Save(appointment4);
            _context.Save(appointment5);
            var otherPatient = new PatientBuilder().Build();
            _context.Save(otherPatient);
            var otherDoctor = new DoctorBuilder().Build();
            _context.Save(otherDoctor);
            var appointment = new AppointmentBuilder(otherPatient.Id, otherDoctor.Id).Build();
            _context.Save(appointment);
            var updateDto = UpdateAppointmentDtoFactory.Create(doctor.Id, otherPatient.Id,date);

            var actual = () => _sut.Update(appointment.Id, updateDto);

            await actual.Should().ThrowExactlyAsync<DoctorCantHaveMoreThanFiveAppointmentPerDayException>();
        }
    }
}
