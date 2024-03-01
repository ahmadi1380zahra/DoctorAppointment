using DoctorAppointment.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Persistance.EF.Appointments
{
    public class AppointmentEntityMap : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_=>_.Id).ValueGeneratedOnAdd();
            builder.Property(_ => _.Date).IsRequired();

            builder.HasOne(_=>_.Doctor)
                .WithMany(_=>_.Appointments)
                .HasForeignKey(_=>_.DoctorId)
                .IsRequired();
            builder.HasOne(_=>_.Patient)
                .WithMany(_=>_.Appointments)
                .HasForeignKey(_=>_.PatientId)
                .IsRequired();
        }
    }
}
