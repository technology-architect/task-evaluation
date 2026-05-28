namespace EmployeeMonitoring.Common.Models
{
    public class EmployeeConsent
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public bool KeystrokeConsentGiven { get; set; }
        public bool ScreenshotConsentGiven { get; set; }
        public DateTime ConsentDate { get; set; }
        public DateTime? RevokedDate { get; set; }
        public string ConsentVersion { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string ConsentHash { get; set; } // For verification
    }
    
    public class ConsentRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public bool KeystrokeCapture { get; set; }
        public bool ScreenshotCapture { get; set; }
        public string Reason { get; set; }
        public ConsentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public string RespondedBy { get; set; }
    }
    
    public enum ConsentStatus
    {
        Pending,
        Approved,
        Denied,
        Revoked
    }
    
    public class ConsentDTO
    {
        public int EmployeeId { get; set; }
        public bool KeystrokeConsentGiven { get; set; }
        public bool ScreenshotConsentGiven { get; set; }
        public DateTime ConsentDate { get; set; }
        public DateTime? RevokedDate { get; set; }
        public string ConsentVersion { get; set; }
    }
}
