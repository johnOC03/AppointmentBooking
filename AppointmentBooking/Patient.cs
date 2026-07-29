namespace ENSE707_AppointmentBooking
{
    public class Patient
    {
        public string Id { get; }
        public string LegalName { get; }
        public string PreferredName { get; }
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PreferredName))
                    return LegalName;
                return PreferredName;
            }
        }

        public Patient(string id, string legalName, string preferredName = "")
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Patient ID is required.");
            if (!IsValidPatientId(id))
                throw new ArgumentException("Patient ID must start with 'P' followed by digits (e.g., P001).");
            if (string.IsNullOrWhiteSpace(legalName))
                throw new ArgumentException("Legal name is required.");

            Id = id;
            LegalName = legalName;
            PreferredName = preferredName;
        }

        private static bool IsValidPatientId(string id)
        {
            if (id.Length < 2)
                return false;
            if (id[0] != 'P')
                return false;
            return id.Substring(1).All(char.IsDigit);
        }
    }
}
