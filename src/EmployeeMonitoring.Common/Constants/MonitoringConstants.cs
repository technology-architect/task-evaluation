namespace EmployeeMonitoring.Common.Constants
{
    public static class MonitoringConstants
    {
        public const string DefaultConfigFile = "config/monitoring.json";
        public const string SecurityConfigFile = "config/security.json";
        
        public const int MaxKeystrokeLogSize = 1048576; // 1 MB
        public const int MaxScreenshotSize = 5242880; // 5 MB
        
        public const string AesGcmAlgorithm = "AES-256-GCM";
        public const int EncryptionKeySize = 32; // 256 bits
        public const int GcmNonceSize = 12; // 96 bits
        public const int GcmTagSize = 16; // 128 bits
        
        public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
        public const string ScreenshotNameFormat = "screenshot_{0:yyyyMMdd_HHmmss}.png";
        public const string KeylogNameFormat = "keylog_{0:yyyyMMdd_HHmmss}.log";
        
        public static class DefaultRetention
        {
            public const int KeylogsRetentionDays = 30;
            public const int ScreenshotsRetentionDays = 90;
            public const int AuditLogsRetentionDays = 365;
        }
        
        public static class DefaultIntervals
        {
            public const int KeystrokeCaptureMsInterval = 5000; // 5 seconds
            public const int ScreenshotCaptureMinInterval = 60; // 1 hour
        }
    }
}
