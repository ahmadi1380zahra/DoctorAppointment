using DoctorAppointment.Contracts.Interfaces;
using DoctorAppointment.Persistance.EF;
using DoctorAppointment.Persistance.EF.Patients;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Exceptions;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Contracts;
using DoctorAppointment.Services.Patients.Contracts.Dtos;
using DoctorAppointment.Services.Patients.Exceptions;
using DoctorAppointment.Test.Tools.Infrastructure.DatabaseConfig.Unit;
using DoctorAppointment.Test.Tools.Patients;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Unit.Tests
{


    public class PatientServiceTests
    {
        private readonly PatientService _sut;
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        public PatientServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = PatientServiceFactory.Create(_context);
        }


        [Fact]
        public async Task Add_adds_a_new_patient_properly()
        {
            var dto = AddPatientDtoFactory.Create();

            await _sut.Add(dto);

            var actual = _readContext.Patients.Single();
            actual.FirstName.Should().Be(dto.FirstName);
            actual.LastName.Should().Be(dto.LastName);
            actual.NationalCode.Should().Be(dto.NationalCode);
        }

        [Fact]
        public async Task Add_throws_PatientsNationalCodeIsReduplicatedException()
        {
            var dummyNationalCode = "123";
            var patient = new PatientBuilder().WithNatinalCode(dummyNationalCode).Build();
            _context.Save(patient);
            var reduplicatePatientDto = AddPatientDtoFactory.Create(dummyNationalCode);

            var actual = () => _sut.Add(reduplicatePatientDto);

            await actual.Should().ThrowExactlyAsync<PatientsNationalCodeIsReduplicatedException>();
        }
        [Fact]
        public async Task Update_updates_a_patient_properly()
        {

            var patient = new PatientBuilder().Build();
            _context.Save(patient);
            var updateDto = UpdatePatientDtoFactory.Create();

            await _sut.Update(patient.Id, updateDto);

            var actual = _readContext.Patients.First(_ => _.Id == patient.Id);
            actual.FirstName.Should().Be(updateDto.FirstName);
            actual.LastName.Should().Be(updateDto.LastName);
            actual.NationalCode.Should().Be(updateDto.NationalCode);

        }
        [Fact]
        public async Task Update_throws_PatientsNationalCodeIsReduplicatedException()
        {
            var dummyNationalCode = "123456";
            var patient = new PatientBuilder().WithNatinalCode(dummyNationalCode).Build();
            var reduplicatePatient = new PatientBuilder().Build();
            _context.Save(patient);
            _context.Save(reduplicatePatient);
            var updateDto = UpdatePatientDtoFactory.Create(dummyNationalCode);

            var actual = () => _sut.Update(reduplicatePatient.Id, updateDto);

            await actual.Should().ThrowExactlyAsync<PatientsNationalCodeIsReduplicatedException>();
        }
        [Fact]
        public async Task Update_throws_PatientIsNotExistedException()
        {
            var dummyId = 21;
            var dto = UpdatePatientDtoFactory.Create();

            var actual = () => _sut.Update(dummyId, dto);

            await actual.Should().ThrowExactlyAsync<PatientIsNotExistedException>();
        }
        [Fact]
        public async Task Delete_deletes_a_patient_properly()
        {

            var patient = new PatientBuilder().Build();
            _context.Save(patient);

            await _sut.Delete(patient.Id);

            var actual = _readContext.Patients.Count();
            actual.Should().Be(0);
        }
        [Fact]
        public async Task Delete_throws_PatientIsNotExistedException()
        {
            var dummyId = 21;
            var actual = () => _sut.Delete(dummyId);

            await actual.Should().ThrowExactlyAsync<PatientIsNotExistedException>();
        }
        [Fact]
        public void Get_gets_all_patients()
        {
            var patient1 = new PatientBuilder().Build();
            var patient2 = new PatientBuilder().Build();
            _context.Save(patient1);
            _context.Save(patient2);

            var actual = _sut.GetAll();

            actual.Count().Should().Be(2);
        }
        [Fact]
        public void GetAll_get_a_doctor_check_valid_data()
        {
            var patient1 = new PatientBuilder().Build();
            _context.Save(patient1);

            var patients = _sut.GetAll();

            var actual = patients.SingleOrDefault();
            actual.FirstName.Should().Be(patient1.FirstName);
            actual.LastName.Should().Be(patient1.LastName);
            actual.NationalCode.Should().Be(patient1.NationalCode);
        }
    }
}









