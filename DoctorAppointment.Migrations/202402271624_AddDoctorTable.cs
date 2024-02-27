using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Migrations
{
    [Migration(202402271624)]
    public class _202402271624_AddDoctorTable  : Migration
    {
        public override void Up()
        {
            Create.Table("Doctors")
                     .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                     .WithColumn("FirstName").AsString(50).NotNullable()
                     .WithColumn("LastName").AsString(50).NotNullable()
                     .WithColumn("Field").AsString(50).NotNullable()
                     .WithColumn("NationalCode").AsString(50).NotNullable();

        }

        public override void Down()
        {
            Delete.Table("Doctors");
        }
    }
}
