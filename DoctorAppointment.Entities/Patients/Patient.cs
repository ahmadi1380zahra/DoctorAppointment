using DoctorAppointment.Entities.Appointments;

namespace DoctorAppointment.Entities.Patients
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public HashSet<Appointment> Appointments { get; set; }
    }
}