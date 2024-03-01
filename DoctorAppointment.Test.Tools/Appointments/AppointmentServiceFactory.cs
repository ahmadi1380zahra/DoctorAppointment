using DoctorAppointment.Persistance.EF;
using DoctorAppointment.Persistance.EF.Appointments;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Appointments;
using DoctorAppointment.Services.Appointments.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools.Appointments
{
    public static class AppointmentServiceFactory
    {
        public static AppointmentService Create(EFDataContext context)
        {
            return new AppointmentAppService(new EFAppointmentRepository(context), new EFUnitOfWork(context));
        } 
    }
}
