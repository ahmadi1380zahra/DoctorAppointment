using DoctorAppointment.Entities.Doctors;
using DoctorAppointment.Persistance.EF;
using DoctorAppointment.Persistance.EF.Doctors;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Contracts;
using DoctorAppointment.Services.Doctors.Contracts.Dto;
using DoctorAppointment.Services.Doctors.Exceptions;
using DoctorAppointment.Test.Tools.Doctors;
using DoctorAppointment.Test.Tools.Infrastructure.DatabaseConfig.Unit;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using System.Numerics;

namespace DoctorAppointment.Services.Unit.Tests
{
    public class DoctorServiceTests
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly DoctorService _sut;
        public DoctorServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = DoctorServiceFactory.Create(_context);
        }

        [Fact]
        public async Task Add_adds_a_new_doctor_properly()
        {
            var dto = AddDoctorDtoFactory.Create();

            await _sut.Add(dto);

            var actual = _readContext.Doctors.Single();
            actual.FirstName.Should().Be(dto.FirstName);
            actual.LastName.Should().Be(dto.LastName);
            actual.Field.Should().Be(dto.Field);
            actual.NationalCode.Should().Be(dto.NationalCode);
        }
        [Fact]
        public async Task Add_throws_ReduplicateNationalCodeException_if_national_code_is_existed()
        {
            var dummyNationalCode = "12345673";
            var doctor = new DoctorBuilder().WithNationalCode(dummyNationalCode).Build();
            _context.Save(doctor);
            var reduplicateDoctorDto = AddDoctorDtoFactory.Create(dummyNationalCode);

            var actual = () => _sut.Add(reduplicateDoctorDto);

            await actual.Should().ThrowExactlyAsync<ReduplicateNationalCodeException>();
        }

        [Fact]
        public async Task Update_updates_doctor_properly()
        {
            var doctor = new DoctorBuilder().Build();
            _context.Save(doctor);
            var updateDto = UpdateDoctorDtoFactory.Create();

            await _sut.Update(doctor.Id, updateDto);

            var actual = _readContext.Doctors.First(_ => _.Id == doctor.Id);
            actual.FirstName.Should().Be(updateDto.FirstName);
            actual.LastName.Should().Be(updateDto.LastName);
            actual.Field.Should().Be(updateDto.Field);
        }
        [Fact]
        public async Task Update_throws_ReduplicateNationalCodeException_if_national_code_is_existed()
        {
            var dummyNationalCode = "12234556666";
            var doctor = new DoctorBuilder().WithNationalCode(dummyNationalCode).Build();
            var reduplicateDoctor = new DoctorBuilder().Build(); ;
            _context.Save(doctor);
            _context.Save(reduplicateDoctor);
            var dto = UpdateDoctorDtoFactory.Create(dummyNationalCode);

            var actual = () => _sut.Update(reduplicateDoctor.Id, dto);

            await actual.Should().ThrowExactlyAsync<ReduplicateNationalCodeException>();
        }

        [Fact]
        public async Task Update_throws_DoctorNotExistedException()
        {
            var id = 10;
            var dto = UpdateDoctorDtoFactory.Create();

            var actual = () => _sut.Update(id, dto);

            await actual.Should().ThrowExactlyAsync<DoctorNotExistedException>();
        }
        [Fact]
        public async Task Delete_deletes_a_doctor_by_id()
        {
            var doctor = new DoctorBuilder().Build();
            _context.Save(doctor);

            await _sut.Delete(doctor.Id);

            var actual = _readContext.Doctors.Count();
            actual.Should().Be(0);
        }
        [Fact]
        public async Task Delete_throws_DoctorNotExistedException()
        {
            var dummyId = 10;

            var actual = () => _sut.Delete(dummyId);

            await actual.Should().ThrowExactlyAsync<DoctorNotExistedException>();
        }
        [Fact]
        public void GetAll_gets_all_doctors_count()
        {
            var doctor1 = new DoctorBuilder().Build();
            var doctor2 = new  DoctorBuilder().Build();
            var doctor3 = new  DoctorBuilder().Build();
            var doctor4 = new  DoctorBuilder().Build();
            _context.Save(doctor1);
            _context.Save(doctor2);
            _context.Save(doctor3);
            _context.Save(doctor4);

            var actual = _sut.GetAll();

            actual.Count().Should().Be(4);
        }
        [Fact]
        public void GetAll_get_a_doctor_check_valid_data()
        {
            
            var doctor = new DoctorBuilder().Build();
            _context.Save(doctor);

            var doctors = _sut.GetAll();

            var actual = doctors.Single();
            actual.FirstName.Should().Be(doctor.FirstName);
            actual.LastName.Should().Be(doctor.LastName);
            actual.NationalCode.Should().Be(doctor.NationalCode);
            actual.Field.Should().Be(doctor.Field);
        }

    }


}