namespace ENSE707_AppointmentBooking
{
    public class Doctor
    {
        public string Id { get; }
        public string FullName { get; }
        public int AvailableSlots { get; private set; }
        public int MaxDailyAppointments { get; }
        public int CurrentDailyAppointments { get; private set; }

        public Doctor(string id, string fullName, int availableSlots, int maxDailyAppointments = 10)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Doctor ID is required.");
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Doctor name is required.");
            if (availableSlots < 0)
                throw new ArgumentException("Available slots cannot be negative.");
            if (maxDailyAppointments <= 0)
                throw new ArgumentException("Maximum daily appointments must be positive.");

            Id = id;
            FullName = fullName;
            AvailableSlots = availableSlots;
            MaxDailyAppointments = maxDailyAppointments;
            CurrentDailyAppointments = 0;
        }

        public bool HasAvailableSlot()
        {
            return AvailableSlots > 0;
        }

        public bool CanAcceptDailyAppointment()
        {
            return CurrentDailyAppointments < MaxDailyAppointments;
        }

        public void ReserveSlot()
        {
            if (!HasAvailableSlot())
                throw new InvalidOperationException("No appointment slots are available.");
            if (!CanAcceptDailyAppointment())
                throw new InvalidOperationException("Maximum daily appointments reached.");
            AvailableSlots--;
            CurrentDailyAppointments++;
        }
    }
}
