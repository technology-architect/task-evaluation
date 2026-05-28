namespace EmployeeMonitoring.Common.Models
{
    public class MonitoringConfig
    {
        public MonitoringSettings Monitoring { get; set; }
        public StorageSettings Storage { get; set; }
        public DatabaseSettings Database { get; set; }
        public SecuritySettings Security { get; set; }
        public LoggingSettings Logging { get; set; }
    }
    
    public class MonitoringSettings
    {
        public bool Enabled { get; set; }
        public KeystrokeSettings KeystrokeCapture { get; set; }
        public ScreenshotSettings ScreenshotCapture { get; set; }
        public RetentionSettings DataRetention { get; set; }
    }
    
    public class KeystrokeSettings
    {
        public bool Enabled { get; set; }
        public int CaptureInterval { get; set; }
        public List<string> ExcludeApplications { get; set; }
        public bool ExcludeFields { get; set; }
    }
    
    public class ScreenshotSettings
    {
        public bool Enabled { get; set; }
        public int IntervalMinutes { get; set; }
        public int Quality { get; set; }
        public string MaxResolution { get; set; }
        public bool IncludeSecondaryMonitors { get; set; }
    }
    
    public class RetentionSettings
    {
        public int KeylogsRetentionDays { get; set; }
        public int ScreenshotsRetentionDays { get; set; }
        public int AuditLogsRetentionDays { get; set; }
        public bool AutoDeleteEnabled { get; set; }
    }
    
    public class StorageSettings
    {
        public string Type { get; set; }
        public string Path { get; set; }
        public EncryptionSettings Encryption { get; set; }
    }
    
    public class EncryptionSettings
    {
        public bool Enabled { get; set; }
        public string Algorithm { get; set; }
        public int KeyRotationDays { get; set; }
    }
    
    public class DatabaseSettings
    {
        public string Provider { get; set; }
        public string ConnectionString { get; set; }
    }
    
    public class SecuritySettings
    {
        public bool EnableMFA { get; set; }
        public int SessionTimeout { get; set; }
        public PasswordPolicySettings PasswordPolicy { get; set; }
    }
    
    public class PasswordPolicySettings
    {
        public int MinLength { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireNumbers { get; set; }
        public bool RequireSpecialChars { get; set; }
    }
    
    public class LoggingSettings
    {
        public string Level { get; set; }
        public AuditLogSettings AuditLog { get; set; }
    }
    
    public class AuditLogSettings
    {
        public bool Enabled { get; set; }
        public bool LogAllAccess { get; set; }
        public bool LogDataExport { get; set; }
    }
}
