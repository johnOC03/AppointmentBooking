namespace ENSE707_AppointmentBooking
{
    public class AppointmentBookingService
    {
        public BookingResult BookAppointment(AppointmentRequest request)
        {
            if (request == null)
                return new BookingResult(false, "Appointment request is missing. Please provide valid appointment details.");

            if (!request.Doctor.HasAvailableSlot())
            {
                return new BookingResult(
                    false,
                    $"Appointment cannot be booked. {request.Doctor.FullName} has no available slots. Please choose another doctor or date.");
            }

            if (!request.Doctor.CanAcceptDailyAppointment())
            {
                return new BookingResult(
                    false,
                    $"Appointment cannot be booked. {request.Doctor.FullName} has reached the maximum daily appointments limit. Please book for another day.");
            }

            request.Doctor.ReserveSlot();
            return new BookingResult(
                true,
                $"Appointment booked successfully for {request.Patient.DisplayName} with {request.Doctor.FullName} on {request.RequestedDate.ToShortDateString()}.");
        }
    }
}
