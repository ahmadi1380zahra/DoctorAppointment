using DoctorAppointment.Entities.Doctors;

namespace DoctorAppointment.Services.Doctors.Contracts;

public interface DoctorRepository
{
    void Add(Doctor doctor);
    Task<Doctor?> FindById(int id);
}