using DoctorAppointment.Persistance.EF;
using DoctorAppointment.Persistance.EF.Patients;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools.Patients
{
    public static class PatientServiceFactory
    {
        public static PatientService Create(EFDataContext context)
        {
            return new PatientAppService(new EFPatientRepository(context),new EFUnitOfWork(context));
        }

    }
}
