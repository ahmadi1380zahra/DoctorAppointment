using DoctorAppointment.Services.Doctors.Contracts.Dto;

namespace DoctorAppointment.Services.Doctors.Contracts;

public interface DoctorService
{
    Task Add(AddDoctorDto dto);
    Task Delete(int id);
    Task Update(int id, UpdateDoctorDto dto);
    List<GetDoctorDto> GetAll();


}