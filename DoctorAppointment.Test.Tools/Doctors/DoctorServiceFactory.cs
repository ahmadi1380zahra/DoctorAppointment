using DoctorAppointment.Persistance.EF.Doctors;
using DoctorAppointment.Persistance.EF;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools.Doctors
{
    public static class DoctorServiceFactory
    {
        public static DoctorService Create(EFDataContext context)
        {
            return new DoctorAppService(new EFDoctorRepository(context), new EFUnitOfWork(context));
        }
    }
}
