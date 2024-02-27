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
            var db=new EFInMemoryDatabase();
            var context=db.CreateDataContext<EFDataContext>();
            var readContext=db.CreateDataContext<EFDataContext>();
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
            var sut = new PatientAppService(new EFPatientRepository(context),new EFUnitOfWork(context));

            await sut.Update(patient.Id, dto);

            var actual = readContext.Patients.First(_=>_.Id==patient.Id);
            actual.FirstName.Should().Be(dto.FirstName);
            actual.LastName.Should().Be(dto.LastName);
            actual.NationalCode.Should().Be(dto.NationalCode);

        }
    }
}









