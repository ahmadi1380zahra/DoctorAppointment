using DoctorAppointment.Entities.Appointments;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Appointments.Contracts.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Persistance.EF.Appointments
{
    public class EFAppointmentRepository : AppointmentRepository
    {
        private readonly EFDataContext _context;
        public EFAppointmentRepository(EFDataContext context)
        {
            _context = context;
        }
        public void Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
        }

        public void Delete(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
        }

        public int DoctorAppointmentsCountsPerDay(int doctorId, DateTime date)
        {
            return _context.Appointments.Count(_ => _.DoctorId == doctorId && _.Date == date);
        }

        public Appointment? Find(int id)
        {
            return _context.Appointments.FirstOrDefault(_ => _.Id == id);
        }

        public async Task<List<GetAppointmentDto>> GetAll()
        {
            IQueryable<Appointment> query =  _context.Appointments;
            List<GetAppointmentDto> appointments = await query.Select(appointment =>  new GetAppointmentDto
            {
                Id = appointment.Id,
                DoctorName = appointment.Doctor.FirstName + appointment.Doctor.LastName,
                PatientName = appointment.Patient.FirstName + appointment.Patient.LastName,
                Date = appointment.Date
            }).ToListAsync();
            return  appointments;
        }

        public bool IsExistDoctor(int doctorId)
        {
            return _context.Doctors.Any(_ => _.Id == doctorId);
        }
        public bool IsExistPatient(int patientId)
        {
            return _context.Patients.Any(_ => _.Id == patientId);
        }

        public void Update(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }
    }
}
