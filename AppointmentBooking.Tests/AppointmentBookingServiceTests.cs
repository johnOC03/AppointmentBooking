using ENSE707_AppointmentBooking;

namespace AppointmentBooking.Tests
{
    [TestClass]
    public class AppointmentBookingServiceTests
    {
        [TestMethod]
        public void BookAppointment_WhenDoctorHasAvailableSlots_ReturnsSuccess()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            BookingResult result = service.BookAppointment(request);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void BookAppointment_WhenDoctorHasNoAvailableSlots_ReturnsFailure()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            BookingResult result = service.BookAppointment(request);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void BookAppointment_WhenSuccessful_DecreasesAvailableSlots()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            service.BookAppointment(request);
            Assert.AreEqual(1, doctor.AvailableSlots);
        }

        [TestMethod]
        public void BookAppointment_WhenFailed_DoesNotDecreaseAvailableSlots()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            service.BookAppointment(request);
            Assert.AreEqual(0, doctor.AvailableSlots);
        }

        [TestMethod]
        public void Doctor_WhenIdIsEmpty_ThrowsException()
        {
            try
            {
                var doctor = new Doctor("", "Dr Mark", 2);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public void Doctor_WhenAvailableSlotsIsNegative_ThrowsException()
        {
            try
            {
                var doctor = new Doctor("D001", "Dr Mark", -1);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public void Patient_WhenIdIsEmpty_ThrowsException()
        {
            try
            {
                var patient = new Patient("", "Diana William");
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public void Patient_WhenPreferredNameExists_DisplayNameUsesPreferredName()
        {
            var patient = new Patient("P001", "Diana William", "Aroha");
            Assert.AreEqual("Aroha", patient.DisplayName);
        }

        [TestMethod]
        public void Patient_WhenPreferredNameMissing_DisplayNameUsesLegalName()
        {
            var patient = new Patient("P001", "Diana William");
            Assert.AreEqual("Diana William", patient.DisplayName);
        }

        [TestMethod]
        public void AppointmentRequest_WhenRequestedDateIsInPast_ThrowsException()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");
            try
            {
                var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(-1));
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public void BookAppointment_WhenSuccessful_ReturnsHelpfulMessage()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William", "Aroha");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            BookingResult result = service.BookAppointment(request);
            StringAssert.Contains(result.Message, "Appointment booked successfully");
            StringAssert.Contains(result.Message, "Aroha");
        }

        [TestMethod]
        public void BookAppointment_WhenNoSlots_ReturnsHelpfulMessage()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            BookingResult result = service.BookAppointment(request);
            StringAssert.Contains(result.Message, "no available slots");
            StringAssert.Contains(result.Message, "choose another doctor or date");
        }

        // New Business Rule Tests

        [TestMethod]
        public void AppointmentRequest_WhenBookingForToday_WithAdvanceNoticeRequired_ThrowsException()
        {
            var doctor = new Doctor("D001", "Dr Mark", 5);
            var patient = new Patient("P001", "Diana William");
            try
            {
                var request = new AppointmentRequest(patient, doctor, DateTime.Today, requiresAdvanceNotice: true);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "one day advance notice");
            }
        }

        [TestMethod]
        public void AppointmentRequest_WhenBookingForToday_WithoutAdvanceNoticeRequired_Succeeds()
        {
            var doctor = new Doctor("D001", "Dr Mark", 5);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today, requiresAdvanceNotice: false);
            Assert.IsNotNull(request);
            Assert.AreEqual(DateTime.Today, request.RequestedDate.Date);
        }

        [TestMethod]
        public void Doctor_WhenMaxDailyAppointmentsReached_CannotBookMoreAppointments()
        {
            var doctor = new Doctor("D001", "Dr Mark", 10, maxDailyAppointments: 2);
            var patient = new Patient("P001", "Diana William");
            var service = new AppointmentBookingService();

            // Book first appointment
            var request1 = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var result1 = service.BookAppointment(request1);
            Assert.IsTrue(result1.Success);

            // Book second appointment
            var request2 = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var result2 = service.BookAppointment(request2);
            Assert.IsTrue(result2.Success);

            // Try to book third appointment - should fail
            var request3 = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var result3 = service.BookAppointment(request3);
            Assert.IsFalse(result3.Success);
            StringAssert.Contains(result3.Message, "maximum daily appointments limit");
        }

        [TestMethod]
        public void Doctor_WhenMaxDailyAppointmentsIsZero_ThrowsException()
        {
            try
            {
                var doctor = new Doctor("D001", "Dr Mark", 5, maxDailyAppointments: 0);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Maximum daily appointments must be positive");
            }
        }

        [TestMethod]
        public void Patient_WhenIdDoesNotStartWithP_ThrowsException()
        {
            try
            {
                var patient = new Patient("A001", "Diana William");
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Patient ID must start with 'P'");
            }
        }

        [TestMethod]
        public void Patient_WhenIdContainsNonDigitsAfterP_ThrowsException()
        {
            try
            {
                var patient = new Patient("P00A", "Diana William");
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Patient ID must start with 'P' followed by digits");
            }
        }

        [TestMethod]
        public void Patient_WhenIdIsValidFormat_CreatesSuccessfully()
        {
            var patient = new Patient("P123", "Diana William");
            Assert.AreEqual("P123", patient.Id);
            Assert.AreEqual("Diana William", patient.LegalName);
        }

        [TestMethod]
        public void BookAppointment_WhenSuccessful_MessageIsActionable()
        {
            var doctor = new Doctor("D001", "Dr Mark", 5);
            var patient = new Patient("P001", "Diana William");
            var appointmentDate = DateTime.Today.AddDays(1);
            var request = new AppointmentRequest(patient, doctor, appointmentDate);
            var service = new AppointmentBookingService();
            
            var result = service.BookAppointment(request);
            
            Assert.IsTrue(result.Success);
            StringAssert.Contains(result.Message, "Appointment booked successfully");
            StringAssert.Contains(result.Message, patient.DisplayName);
            StringAssert.Contains(result.Message, doctor.FullName);
            StringAssert.Contains(result.Message, appointmentDate.ToShortDateString());
        }

        [TestMethod]
        public void BookAppointment_WhenFailed_MessageIsActionable()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            
            var result = service.BookAppointment(request);
            
            Assert.IsFalse(result.Success);
            StringAssert.Contains(result.Message, "cannot be booked");
            StringAssert.Contains(result.Message, "choose another doctor or date");
        }

        [TestMethod]
        public void BookAppointment_WhenSuccessful_IncrementsDailyAppointmentCount()
        {
            var doctor = new Doctor("D001", "Dr Mark", 5);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            
            
            Assert.AreEqual(0, doctor.CurrentDailyAppointments);
            service.BookAppointment(request);
            Assert.AreEqual(1, doctor.CurrentDailyAppointments);
        }

        // Additional Comprehensive Tests for New Business Rules

        [TestMethod]
        public void BookAppointment_WhenBookingForTomorrow_IsAccepted()
        {
            var doctor = new Doctor("D001", "Dr Mark", 5);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            
            var result = service.BookAppointment(request);
            
            Assert.IsTrue(result.Success);
            Assert.AreEqual(4, doctor.AvailableSlots);
            StringAssert.Contains(result.Message, "successfully");
        }

        [TestMethod]
        public void BookAppointment_WhenBookingForToday_WithAdvanceNotice_IsRejected()
        {
            var doctor = new Doctor("D001", "Dr Mark", 5);
            var patient = new Patient("P001", "Diana William");
            
            try
            {
                var request = new AppointmentRequest(patient, doctor, DateTime.Today, requiresAdvanceNotice: true);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "advance notice");
            }
        }

        [TestMethod]
        public void BookAppointment_WhenDoctorHasNoSlots_BookingFailsWithClearMessage()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            
            var result = service.BookAppointment(request);
            
            Assert.IsFalse(result.Success);
            StringAssert.Contains(result.Message, "Dr Mark");
            StringAssert.Contains(result.Message, "no available slots");
            StringAssert.Contains(result.Message, "choose another doctor or date");
        }

        [TestMethod]
        public void BookAppointment_SuccessMessage_IncludesDoctorName()
        {
            var doctor = new Doctor("D001", "Dr Sarah Smith", 5);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            
            var result = service.BookAppointment(request);
            
            Assert.IsTrue(result.Success);
            StringAssert.Contains(result.Message, "Dr Sarah Smith");
        }

        [TestMethod]
        public void BookAppointment_SuccessMessage_IncludesPatientDisplayName()
        {
            var doctor = new Doctor("D001", "Dr Mark", 5);
            var patient = new Patient("P001", "Diana William Johnson", "Di");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            
            var result = service.BookAppointment(request);
            
            Assert.IsTrue(result.Success);
            StringAssert.Contains(result.Message, "Di");
            Assert.IsFalse(result.Message.Contains("Diana William Johnson"));
        }

        [TestMethod]
        public void Patient_WhenIdIsInvalidFormat_IsRejected()
        {
            try
            {
                var patient = new Patient("123", "Diana William");
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Patient ID must start with 'P'");
            }
        }

        [TestMethod]
        public void BookAppointment_WhenFailed_SlotCountRemainsUnchanged()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            
            int initialSlots = doctor.AvailableSlots;
            var result = service.BookAppointment(request);
            
            Assert.IsFalse(result.Success);
            Assert.AreEqual(initialSlots, doctor.AvailableSlots);
            Assert.AreEqual(0, doctor.CurrentDailyAppointments);
        }

        [TestMethod]
        public void BookAppointment_MultipleSuccessfulBookings_DecrementsSlotCountCorrectly()
        {
            var doctor = new Doctor("D001", "Dr Mark", 5);
            var patient1 = new Patient("P001", "Diana William");
            var patient2 = new Patient("P002", "John Smith");
            var patient3 = new Patient("P003", "Sarah Jones");
            var service = new AppointmentBookingService();
            
            var request1 = new AppointmentRequest(patient1, doctor, DateTime.Today.AddDays(1));
            var request2 = new AppointmentRequest(patient2, doctor, DateTime.Today.AddDays(1));
            var request3 = new AppointmentRequest(patient3, doctor, DateTime.Today.AddDays(1));
            
            service.BookAppointment(request1);
            Assert.AreEqual(4, doctor.AvailableSlots);
            
            service.BookAppointment(request2);
            Assert.AreEqual(3, doctor.AvailableSlots);
            
            service.BookAppointment(request3);
            Assert.AreEqual(2, doctor.AvailableSlots);
        }

        [TestMethod]
        public void BookAppointment_WhenMaxDailyAppointmentsReached_DailyCountDoesNotIncrement()
        {
            var doctor = new Doctor("D001", "Dr Mark", 10, maxDailyAppointments: 1);
            var patient1 = new Patient("P001", "Diana William");
            var patient2 = new Patient("P002", "John Smith");
            var service = new AppointmentBookingService();
            
            var request1 = new AppointmentRequest(patient1, doctor, DateTime.Today.AddDays(1));
            var result1 = service.BookAppointment(request1);
            Assert.IsTrue(result1.Success);
            Assert.AreEqual(1, doctor.CurrentDailyAppointments);
            
            var request2 = new AppointmentRequest(patient2, doctor, DateTime.Today.AddDays(1));
            var result2 = service.BookAppointment(request2);
            Assert.IsFalse(result2.Success);
            Assert.AreEqual(1, doctor.CurrentDailyAppointments);
        }

        [TestMethod]
        public void BookAppointment_WhenMaxDailyLimitReached_MessageIsActionable()
        {
            var doctor = new Doctor("D001", "Dr Mark", 10, maxDailyAppointments: 1);
            var patient1 = new Patient("P001", "Diana William");
            var patient2 = new Patient("P002", "John Smith");
            var service = new AppointmentBookingService();
            
            var request1 = new AppointmentRequest(patient1, doctor, DateTime.Today.AddDays(1));
            service.BookAppointment(request1);
            
            var request2 = new AppointmentRequest(patient2, doctor, DateTime.Today.AddDays(1));
            var result = service.BookAppointment(request2);
            
            Assert.IsFalse(result.Success);
            StringAssert.Contains(result.Message, "maximum daily appointments limit");
            StringAssert.Contains(result.Message, "book for another day");
        }

        [TestMethod]
        public void Patient_WhenIdHasVariousValidFormats_IsAccepted()
        {
            var patient1 = new Patient("P1", "Patient One");
            Assert.AreEqual("P1", patient1.Id);
            
            var patient2 = new Patient("P001", "Patient Two");
            Assert.AreEqual("P001", patient2.Id);
            
            var patient3 = new Patient("P12345", "Patient Three");
            Assert.AreEqual("P12345", patient3.Id);
        }

        [TestMethod]
        public void BookAppointment_WhenRequestIsNull_MessageIsActionable()
        {
            var service = new AppointmentBookingService();
            
            var result = service.BookAppointment(null);
            
            Assert.IsFalse(result.Success);
            StringAssert.Contains(result.Message, "missing");
            StringAssert.Contains(result.Message, "valid appointment details");
        }

        [TestMethod]
        public void BookAppointment_SuccessMessage_IncludesAppointmentDate()
        {
            var doctor = new Doctor("D001", "Dr Mark", 5);
            var patient = new Patient("P001", "Diana William");
            var appointmentDate = DateTime.Today.AddDays(2);
            var request = new AppointmentRequest(patient, doctor, appointmentDate);
            var service = new AppointmentBookingService();
            
            var result = service.BookAppointment(request);
            
            Assert.IsTrue(result.Success);
            StringAssert.Contains(result.Message, appointmentDate.ToShortDateString());
        }
    }
}
