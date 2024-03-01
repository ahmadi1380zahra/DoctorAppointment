using DoctorAppointment.Contracts.Interfaces;
using DoctorAppointment.Entities.Doctors;
using DoctorAppointment.Entities.Patients;
using DoctorAppointment.Persistance.EF;
using DoctorAppointment.Persistance.EF.Appointments;
using DoctorAppointment.Persistance.EF.Doctors;
using DoctorAppointment.Persistance.EF.Patients;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Services.Appointments;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Contracts;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Contracts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile("appsettings.json");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EFDataContext>(
    options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<EFDataContext>();
builder.Services.AddScoped<UnitOfWork, EFUnitOfWork>();
builder.Services.AddScoped<DoctorService, DoctorAppService>();
builder.Services.AddScoped<DoctorRepository, EFDoctorRepository>();
builder.Services.AddScoped<PatientService, PatientAppService>();
builder.Services.AddScoped<PatientRepository, EFPatientRepository>();
builder.Services.AddScoped<AppointmentService, AppointmentAppService>();
builder.Services.AddScoped<AppointmentRepository, EFAppointmentRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
