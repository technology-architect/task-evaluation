namespace EmployeeMonitoring.Common.Models
{
    public class Screenshot
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CapturedAt { get; set; }
        public string Filename { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public bool IsEncrypted { get; set; }
        public string EncryptionKeyId { get; set; }
        public string Hash { get; set; } // SHA-256 hash for integrity verification
    }
    
    public class Keylog
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CapturedAt { get; set; }
        public string Application { get; set; }
        public string Window { get; set; }
        public string KeyData { get; set; } // Encrypted
        public bool IsEncrypted { get; set; }
        public string EncryptionKeyId { get; set; }
        public long DataSize { get; set; }
    }
    
    public class AuditLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; }
        public string UserId { get; set; }
        public string TargetEmployeeId { get; set; }
        public string Action { get; set; }
        public string ResourceId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }
    }
    
    public class ScreenshotDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CapturedAt { get; set; }
        public string Filename { get; set; }
        public long FileSize { get; set; }
        public bool Encrypted { get; set; }
        public string Url { get; set; }
    }
    
    public class KeylogDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CapturedAt { get; set; }
        public string Application { get; set; }
        public string Window { get; set; }
        public string KeyData { get; set; }
        public bool Decrypted { get; set; }
    }
}
