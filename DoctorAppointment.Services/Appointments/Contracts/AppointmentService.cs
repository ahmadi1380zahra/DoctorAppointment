using DoctorAppointment.Services.Appointments.Contracts.Dtos;
using DoctorAppointment.Services.Doctors.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public interface AppointmentService
    {
        Task Add(AddAppointmentDto dto);
        Task Delete(int id);
        Task<List<GetAppointmentDto>> GetAll();
        Task Update(int id, UpdateAppointmentDto updateDto);
    }
}
