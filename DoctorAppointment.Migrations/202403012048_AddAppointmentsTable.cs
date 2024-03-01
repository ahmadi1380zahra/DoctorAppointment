using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Migrations
{
    [Migration(202403012048)]
    public class _202403012048_AddAppointmentsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Appointments")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("Date").AsDate().NotNullable()
                .WithColumn("DoctorId").AsInt32().NotNullable()
                 .ForeignKey("FK_Appointments_Doctors", "Doctors", "Id")
                .WithColumn("PatientId").AsInt32().NotNullable()
                 .ForeignKey("FK_Appointments_Patients", "Patients", "Id");
        }
        public override void Down()
        {
            Delete.Table("Appointments");
        }


    }
}
