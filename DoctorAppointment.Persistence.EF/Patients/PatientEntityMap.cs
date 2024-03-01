using DoctorAppointment.Entities.Patients;
using DoctorAppointment.Services.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Persistance.EF.Patients
{
    public class PatientEntityMap : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
            builder.Property(_=>_.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(_=>_.LastName).IsRequired().HasMaxLength(50);
            builder.Property(_=>_.NationalCode).IsRequired().HasMaxLength(10);

      
        }
    }
}
