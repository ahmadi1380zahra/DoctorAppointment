using DoctorAppointment.Contracts.Interfaces;
using DoctorAppointment.Entities.Patients;
using DoctorAppointment.Services.Patients.Contracts;
using DoctorAppointment.Services.Patients.Contracts.Dtos;
using DoctorAppointment.Services.Patients.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Services.Patients
{
    public class PatientAppService : PatientService
    {
        private readonly PatientRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        public PatientAppService(PatientRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Add(AddPatientDto dto)
        {
            if (_repository.IsExistNationalCode(dto.NationalCode))
            {
                throw new PatientsNationalCodeIsReduplicatedException();
            }
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NationalCode = dto.NationalCode,
            };
            _repository.Add(patient);
            await _unitOfWork.Complete();
        }

        public async Task Delete(int id)
        {
            var patient = _repository.Find(id);
            if(patient is null)
            {
                throw new PatientIsNotExistedException();
            }
            _repository.Delete(patient);
            await _unitOfWork.Complete();
        }

        public List<GetPatientDto> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task Update(int id, UpdatePatientDto dto)
        {
            var patient = _repository.Find(id);
            if (patient is null)
            {
                throw new PatientIsNotExistedException();
            }
            if (_repository.IsExistNationalCodeExceptItSelf(id, dto.NationalCode))
            {
                throw new PatientsNationalCodeIsReduplicatedException();
            }
            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.NationalCode = dto.NationalCode;
            _repository.Update(patient);
            await _unitOfWork.Complete();
        }
    }
}
