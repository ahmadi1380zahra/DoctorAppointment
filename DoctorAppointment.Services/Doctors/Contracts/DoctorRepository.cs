using DoctorAppointment.Entities.Doctors;
using DoctorAppointment.Services.Doctors.Contracts.Dto;

namespace DoctorAppointment.Services.Doctors.Contracts;

public interface DoctorRepository
{
    void Add(Doctor doctor);
    Task<Doctor?> FindById(int id);
    bool IsExistNationalCode(string nationalCode);
    bool IsExistNationalCodeExceptItSelf(int id,string nationalCode);
    void Delete(Doctor doctor);
    List<GetDoctorDto> GetAll();

}