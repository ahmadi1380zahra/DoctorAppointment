using DoctorAppointment.Contracts.Interfaces;
using DoctorAppointment.Entities.Appointments;
using DoctorAppointment.Entities.Doctors;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Appointments.Contracts.Dtos;
using DoctorAppointment.Services.Appointments.Exceptions;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Appointments
{
    public class AppointmentAppService : AppointmentService
    {
        private readonly AppointmentRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        public AppointmentAppService(AppointmentRepository repository,UnitOfWork unitOfWork)
        {
            _repository = repository; 
            _unitOfWork = unitOfWork;

        }
        public async Task Add(AddAppointmentDto dto)
        {
            if (!_repository.IsExistDoctor(dto.DoctorId))
            {
                throw new DoctorIsNotExistedException();
            }
            if (!_repository.IsExistPatient(dto.PatientId))
            {
                throw new PatientIsNotExistedException();
            }
            if (_repository.DoctorAppointmentsCountsPerDay(dto.DoctorId,dto.Date)>=5)
            {
                throw new DoctorCantHaveMoreThanFiveAppointmentPerDayException();
            }
            var appointment = new Appointment
            {
                DoctorId=dto.DoctorId,
                PatientId=dto.PatientId,
                Date=dto.Date
            };
            _repository.Add(appointment);
            await _unitOfWork.Complete();
        }

        public async Task Delete(int id)
        {
            var appointment = _repository.Find(id);
            if (appointment == null)
            {
                throw new AppointmentIsNotExistException();
            }
            _repository.Delete(appointment);
            await _unitOfWork.Complete();
        }

        public async Task<List<GetAppointmentDto>> GetAll()
        {
           return await _repository.GetAll();
        }

        public async Task Update(int id, UpdateAppointmentDto updateDto)
        {
            var appointment = _repository.Find(id);
            if (appointment is null)
            {
                throw new AppointmentIsNotExistToUpdateException();
            }
            if (!_repository.IsExistDoctor(updateDto.DoctorId))
            {
                throw new DoctorIsNotExistedException();
            }
            if (!_repository.IsExistPatient(updateDto.PatientId))
            {
                throw new PatientIsNotExistedException();
            }
            if (_repository.DoctorAppointmentsCountsPerDay(updateDto.DoctorId, updateDto.Date) >= 5)
            {
                throw new DoctorCantHaveMoreThanFiveAppointmentPerDayException();
            }
            appointment.Date = updateDto.Date;
            appointment.PatientId=updateDto.PatientId;
            appointment.DoctorId=updateDto.DoctorId;
            _repository.Update(appointment);
            await _unitOfWork.Complete();
        }
    }
}
