using DoctorAppointment.Entities.Doctors;
using DoctorAppointment.Persistance.EF;
using DoctorAppointment.Persistance.EF.Doctors;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Contracts.Dto;
using DoctorAppointment.Services.Doctors.Exceptions;
using DoctorAppointment.Test.Tools.Infrastructure.DatabaseConfig.Unit;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using System.Numerics;

namespace DoctorAppointment.Services.Unit.Tests
{
    public class DoctorServiceTests
    {
        [Fact]
        public async Task Add_adds_a_new_doctor_properly()
        {
            //arrange
            var dto = new AddDoctorDto
            {
                FirstName = "dummy-first-name",
                LastName = "dummy-last-name",
                Field = "heart",
                NationalCode = "2283182033"
            };
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var readContext = db.CreateDataContext<EFDataContext>();
            var sut = new DoctorAppService(new EFDoctorRepository(context), new EFUnitOfWork(context));

            //act
            await sut.Add(dto);

            //assert
            var actual = readContext.Doctors.Single();
            actual.FirstName.Should().Be(dto.FirstName);
            actual.LastName.Should().Be(dto.LastName);
            actual.Field.Should().Be(dto.Field);
            actual.NationalCode.Should().Be(dto.NationalCode);
        }
        [Fact]
        public async Task Add_throws_ReduplicateNationalCodeException_if_national_code_is_existed()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var readContext = db.CreateDataContext<EFDataContext>();
            var doctor = new Doctor
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                Field = "heart",
                NationalCode = "12345673"
            };
            context.Save(doctor);
            var reduplicateDoctorDto = new AddDoctorDto
            {
                FirstName = "dummy-first-name",
                LastName = "dummy-last-name",
                Field = "heart",
                NationalCode = "12345673"
            };
            var sut = new DoctorAppService(new EFDoctorRepository(context), new EFUnitOfWork(context));

            var actual = () => sut.Add(reduplicateDoctorDto);

            await actual.Should().ThrowExactlyAsync<ReduplicateNationalCodeException>();
        }

        [Fact]
        public async Task Update_updates_doctor_properly()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var readContext = db.CreateDataContext<EFDataContext>();
            //arrange
            var doctor = new Doctor
            {
                FirstName = "dummy-first-name",
                LastName = "dummy-last-name",
                Field = "heart",
                NationalCode = "2283182033"
            };
            context.Save(doctor);
            var sut = new DoctorAppService(new EFDoctorRepository(context), new EFUnitOfWork(context));
            var updateDto = new UpdateDoctorDto
            {
                FirstName = "updated-dummy-first-name",
                LastName = "updated-dummy-last-name",
                Field = "child",
                NationalCode = "2283182033"
            };

            //act
            await sut.Update(doctor.Id, updateDto);

            //assert
            var actual = readContext.Doctors.First(_ => _.Id == doctor.Id);
            actual.FirstName.Should().Be(updateDto.FirstName);
            actual.LastName.Should().Be(updateDto.LastName);
            actual.Field.Should().Be(updateDto.Field);
        }
        [Fact]
        public async Task Update_throws_ReduplicateNationalCodeException_if_national_code_is_existed()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var doctor = new Doctor
            {
                FirstName = "dummy-first-name",
                LastName = "dummy-last-name",
                Field = "heart",
                NationalCode = "2283182033"
            };
            var reduplicateDoctor = new Doctor
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                Field = "heart",
                NationalCode = "22832899"
            };
            context.Save(doctor);
            context.Save(reduplicateDoctor);
            var dto = new UpdateDoctorDto
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                Field = "heart",
                NationalCode = "2283182033"
            };
            var sut = new DoctorAppService(new EFDoctorRepository(context), new EFUnitOfWork(context));

            var actual = () => sut.Update(reduplicateDoctor.Id, dto);

            await actual.Should().ThrowExactlyAsync<ReduplicateNationalCodeException>();
        }

        [Fact]
        public async Task Update_throws_DoctorNotExistedException()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var id = 10;
            var sut = new DoctorAppService(new EFDoctorRepository(context), new EFUnitOfWork(context));
            var dto = new UpdateDoctorDto
            {
                FirstName = "dummy-first-name",
                LastName = "dummy-last-name",
                Field = "heart",
                NationalCode = "2283182033"
            };

            var actual = () => sut.Update(id, dto);


            await actual.Should().ThrowExactlyAsync<DoctorNotExistedException>();

        }
        [Fact]
        public async Task Delete_deletes_a_doctor_by_id()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var readContext = db.CreateDataContext<EFDataContext>();
            var doctor = new Doctor
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                Field = "dentist",
                NationalCode = "12345"
            };
            context.Save(doctor);
            var sut = new DoctorAppService(new EFDoctorRepository(context), new EFUnitOfWork(context));

            await sut.Delete(doctor.Id);

            var actual = readContext.Doctors.Count();
            actual.Should().Be(0);
        }
        [Fact]
        public async Task Delete_throws_DoctorNotExistedException()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var id = 10;
            var sut = new DoctorAppService(new EFDoctorRepository(context), new EFUnitOfWork(context));

            var actual = () => sut.Delete(id);

            await actual.Should().ThrowExactlyAsync<DoctorNotExistedException>();
        }
        [Fact]
        public void GetAll_gets_all_doctors_count()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var sut = new DoctorAppService(new EFDoctorRepository(context), new EFUnitOfWork(context));
            var doctor1 = new Doctor
            {
                FirstName = "zahra",
                LastName = "ahmadi",
                NationalCode = "11233",
                Field = "heart"

            };
            var doctor2 = new Doctor
            {
                FirstName = "sara",
                LastName = "ss",
                NationalCode = "12323",
                Field = "dentist"

            };
            var doctor3 = new Doctor
            {
                FirstName = "sanaz",
                LastName = "aaa",
                NationalCode = "12s33",
                Field = "general"
            };
            var doctor4 = new Doctor
            {
                FirstName = "sanaz",
                LastName = "aaa",
                NationalCode = "12s334",
                Field = "general"
            };
            context.Save(doctor1);
            context.Save(doctor2);
            context.Save(doctor3);
            context.Save(doctor4);

            var actual = sut.GetAll();

            actual.Count().Should().Be(4);
        }
        [Fact]
        public void GetAll_get_a_doctor_check_valid_data()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var doctor = new Doctor
            {
                FirstName = "sanaz",
                LastName = "ahmadi",
                NationalCode = "12",
                Field = "general"
            };
            context.Save(doctor);
            var sut = new DoctorAppService(new EFDoctorRepository(context), new EFUnitOfWork(context));

            var actual = sut.GetAll();

            actual.Single().FirstName.Should().Be("sanaz");
            actual.Single().LastName.Should().Be("ahmadi");
            actual.Single().NationalCode.Should().Be("12");
            actual.Single().Field.Should().Be("general");
        }

    }


}