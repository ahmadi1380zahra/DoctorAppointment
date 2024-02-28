using DoctorAppointment.Contracts.Interfaces;
using DoctorAppointment.Persistance.EF;
using DoctorAppointment.Persistance.EF.Patients;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Doctors.Exceptions;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Contracts.Dtos;
using DoctorAppointment.Services.Patients.Exceptions;
using DoctorAppointment.Test.Tools.Infrastructure.DatabaseConfig.Unit;
using FluentAssertions;
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
        [Fact]
        public async Task Add_adds_a_new_patient_properly()
        {
            var dto = new AddPatientDto
            {
                FirstName = "zahra",
                LastName = "Ahmadi",
                NationalCode = "123"
            };
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var readContext = db.CreateDataContext<EFDataContext>();
            var sut = new PatientAppService(new EFPatientRepository(context), new EFUnitOfWork(context));


            await sut.Add(dto);

            var actual = readContext.Patients.Single();
            actual.FirstName.Should().Be(dto.FirstName);
            actual.LastName.Should().Be(dto.LastName);
            actual.NationalCode.Should().Be(dto.NationalCode);
        }

        [Fact]
        public async Task Add_throws_PatientsNationalCodeIsReduplicatedException()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var sut = new PatientAppService(new EFPatientRepository(context), new EFUnitOfWork(context));
            var patient = new Patient
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                NationalCode = "123"
            };
            context.Save(patient);
            var reduplicatePatientDto = new AddPatientDto
            {
                FirstName = "sara",
                LastName = "qasemi",
                NationalCode = "123"
            };

            var actual = () => sut.Add(reduplicatePatientDto);

            await actual.Should().ThrowExactlyAsync<PatientsNationalCodeIsReduplicatedException>();
        }
        [Fact]
        public async Task Update_updates_a_patient_properly()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var readContext = db.CreateDataContext<EFDataContext>();
            var patient = new Patient
            {
                FirstName = "sara",
                LastName = "qasemi",
                NationalCode = "123"
            };
            context.Save(patient);
            var dto = new UpdatePatientDto
            {
                FirstName = "zizi",
                LastName = "haqiqat",
                NationalCode = "123"
            };
            var sut = new PatientAppService(new EFPatientRepository(context), new EFUnitOfWork(context));

            await sut.Update(patient.Id, dto);

            var actual = readContext.Patients.First(_ => _.Id == patient.Id);
            actual.FirstName.Should().Be(dto.FirstName);
            actual.LastName.Should().Be(dto.LastName);
            actual.NationalCode.Should().Be(dto.NationalCode);

        }
        [Fact]
        public async Task Update_throws_PatientsNationalCodeIsReduplicatedException()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var patient = new Patient
            {
                FirstName = "sara",
                LastName = "qasemi",
                NationalCode = "123"
            };
            var reduplicatePatient = new Patient
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                NationalCode = "1234"
            };
            context.Save(patient);
            context.Save(reduplicatePatient);
            var updateDto = new UpdatePatientDto
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                NationalCode = "123"
            };
            var sut = new PatientAppService(new EFPatientRepository(context), new EFUnitOfWork(context));

            //var actual = () => sut.Update(10, updateDto);
            var actual = () => sut.Update(reduplicatePatient.Id, updateDto);

            await actual.Should().ThrowExactlyAsync<PatientsNationalCodeIsReduplicatedException>();
        }
        [Fact]
        public async Task Update_throws_PatientIsNotExistedException()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var sut = new PatientAppService(new EFPatientRepository(context), new EFUnitOfWork(context));
            var dto = new UpdatePatientDto
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                NationalCode = "123"
            };

            var actual = () => sut.Update(11, dto);

            await actual.Should().ThrowExactlyAsync<PatientIsNotExistedException>();
        }
        [Fact]
        public async Task Delete_deletes_a_patient_properly()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var readContext = db.CreateDataContext<EFDataContext>();
            var sut = new PatientAppService(new EFPatientRepository(context), new EFUnitOfWork(context));
            var patient = new Patient
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                NationalCode = "123"
            };
            context.Save(patient);

            await sut.Delete(patient.Id);

            var actual = readContext.Patients.Count();
            actual.Should().Be(0);
        }
        [Fact]
        public async Task Delete_throws_PatientIsNotExistedException()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var sut = new PatientAppService(new EFPatientRepository(context), new EFUnitOfWork(context));

            var actual = () => sut.Delete(10);

            await actual.Should().ThrowExactlyAsync<PatientIsNotExistedException>();
        }
        [Fact]
        public void Get_gets_all_patients()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var patient1 = new Patient
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                NationalCode = "123"
            };
            var patient2 = new Patient
            {
                FirstName = "sara",
                LastName = "qasemi",
                NationalCode = "12w3"
            };

            context.Save(patient1);
            context.Save(patient2);
            var sut = new PatientAppService(new EFPatientRepository(context), new EFUnitOfWork(context));

            var actual = sut.GetAll();

            actual.Count().Should().Be(2);
        }
        [Fact]
        public void GetAll_get_a_doctor_check_valid_data()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var patient1 = new Patient
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                NationalCode = "123"
            };
            context.Save(patient1);
            var sut = new PatientAppService(new EFPatientRepository(context), new EFUnitOfWork(context));

            var actual = sut.GetAll();

            actual.Single().FirstName.Should().Be(patient1.FirstName);
            actual.Single().LastName.Should().Be(patient1.LastName);
            actual.Single().NationalCode.Should().Be(patient1.NationalCode);
        }
    }
}









