using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Migrations
{
    [Migration(202403012008)]
    public class _202403012008_AddPatientTable : Migration
    {
        public override void Up()
        {
            Create.Table("Patients")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FirstName").AsString(50).NotNullable()
                .WithColumn("LastName").AsString(50).NotNullable()
                .WithColumn("NationalCode").AsString(10).NotNullable();

        }
        public override void Down()
        {
            Delete.Table("Patients");
        }

       
    }
}
