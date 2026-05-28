using EmployeeMonitoring.Common.Models;

namespace EmployeeMonitoring.Services.Interfaces
{
    public interface IMonitoringService
    {
        Task<bool> StartMonitoringAsync(int employeeId);
        Task<bool> StopMonitoringAsync(int employeeId);
        Task<Screenshot> CaptureScreenshotAsync(int employeeId);
        Task<List<Keylog>> CaptureKeystrokesAsync(int employeeId);
        Task<bool> IsMonitoringActiveAsync(int employeeId);
    }
    
    public interface IConsentService
    {
        Task<EmployeeConsent> GetConsentAsync(int employeeId);
        Task<ConsentRequest> RequestConsentAsync(int employeeId, bool keystrokeCapture, bool screenshotCapture, string reason);
        Task<bool> GrantConsentAsync(int employeeId, bool keystrokeConsent, bool screenshotConsent);
        Task<bool> RevokeConsentAsync(int employeeId);
        Task<bool> HasValidConsentAsync(int employeeId, string captureType);
    }
    
    public interface IAuditService
    {
        Task LogAccessAsync(string userId, string targetEmployeeId, string action, string resourceId, 
            string ipAddress, string userAgent, string status, string details = null);
        Task LogConfigurationChangeAsync(string userId, string change, string details);
        Task LogAuthenticationAsync(string username, string status, string ipAddress);
        Task<List<AuditLog>> GetAuditLogsAsync(string eventType = null, string userId = null, 
            DateTime? from = null, DateTime? to = null, int page = 1, int pageSize = 50);
    }
    
    public interface IDataRetentionService
    {
        Task DeleteExpiredDataAsync();
        Task DeleteEmployeeDataAsync(int employeeId);
        Task<long> GetStorageUsageAsync();
    }
}
