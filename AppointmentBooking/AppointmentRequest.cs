namespace ENSE707_AppointmentBooking
{
    public class AppointmentRequest
    {
        public Patient Patient { get; }
        public Doctor Doctor { get; }
        public DateTime RequestedDate { get; }
        public bool RequiresAdvanceNotice { get; }

        public AppointmentRequest(Patient patient, Doctor doctor, DateTime requestedDate, bool requiresAdvanceNotice = true)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            if (requestedDate.Date < DateTime.Today)
                throw new ArgumentException("Requested appointment date cannot be in the past.");
            
            RequiresAdvanceNotice = requiresAdvanceNotice;
            if (RequiresAdvanceNotice && requestedDate.Date == DateTime.Today)
                throw new ArgumentException("Appointment requires at least one day advance notice and cannot be booked for today.");
            
            RequestedDate = requestedDate;
        }
    }
}
