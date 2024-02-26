
using DoctorAppointment.Entities.Doctors;
using DoctorAppointment.Persistance.EF;
using DoctorAppointment.Persistance.EF.Doctors;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Contracts.Dto;
using DoctorAppointment.Test.Tools.Infrastructure.DatabaseConfig.Unit;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace DoctorAppointment.Services.Unit.Tests;

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
            Field = "heart"
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
            Field = "heart"
        };
        context.Save(doctor);
        var sut = new DoctorAppService(new EFDoctorRepository(context), new EFUnitOfWork(context));
        var updateDto = new UpdateDoctorDto
        {
            FirstName = "updated-dummy-first-name",
            LastName = "updated-dummy-last-name",
            Field = "child"
        };
        
        //act
        await sut.Update(doctor.Id, updateDto);
        
        //assert
        var actual = readContext.Doctors.First(_=>_.Id == doctor.Id);
        actual.FirstName.Should().Be(updateDto.FirstName);
        actual.LastName.Should().Be(updateDto.LastName);
        actual.Field.Should().Be(updateDto.Field);
    }
}










