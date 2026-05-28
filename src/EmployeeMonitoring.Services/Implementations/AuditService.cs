using Microsoft.EntityFrameworkCore;
using EmployeeMonitoring.Common.Models;
using EmployeeMonitoring.Services.Interfaces;
using EmployeeMonitoring.Infrastructure.Data;

namespace EmployeeMonitoring.Services.Implementations
{
    public class AuditService : IAuditService
    {
        private readonly MonitoringDbContext _dbContext;
        private readonly ILogger<AuditService> _logger;
        
        public AuditService(MonitoringDbContext dbContext, ILogger<AuditService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        /// <summary>
        /// Logs data access events
        /// </summary>
        public async Task LogAccessAsync(string userId, string targetEmployeeId, string action, 
            string resourceId, string ipAddress, string userAgent, string status, string details = null)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    Timestamp = DateTime.UtcNow,
                    EventType = "DataAccess",
                    UserId = userId,
                    TargetEmployeeId = targetEmployeeId,
                    Action = action,
                    ResourceId = resourceId,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    Status = status,
                    Details = details
                };
                
                _dbContext.AuditLogs.Add(auditLog);
                await _dbContext.SaveChangesAsync();
                
                _logger.LogInformation(
                    $"Audit log created: User={userId}, Action={action}, Employee={targetEmployeeId}, Status={status}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging access event");
                // Don't throw - audit logging should not break main functionality
            }
        }
        
        /// <summary>
        /// Logs configuration changes
        /// </summary>
        public async Task LogConfigurationChangeAsync(string userId, string change, string details)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    Timestamp = DateTime.UtcNow,
                    EventType = "ConfigurationChange",
                    UserId = userId,
                    Action = change,
                    Status = "Success",
                    Details = details
                };
                
                _dbContext.AuditLogs.Add(auditLog);
                await _dbContext.SaveChangesAsync();
                
                _logger.LogInformation($"Configuration change logged: User={userId}, Change={change}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging configuration change");
            }
        }
        
        /// <summary>
        /// Logs authentication events
        /// </summary>
        public async Task LogAuthenticationAsync(string username, string status, string ipAddress)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    Timestamp = DateTime.UtcNow,
                    EventType = "Authentication",
                    UserId = username,
                    Action = "Login",
                    IpAddress = ipAddress,
                    Status = status
                };
                
                _dbContext.AuditLogs.Add(auditLog);
                await _dbContext.SaveChangesAsync();
                
                _logger.LogInformation($"Authentication event logged: User={username}, Status={status}, IP={ipAddress}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging authentication event");
            }
        }
        
        /// <summary>
        /// Retrieves audit logs with optional filtering
        /// </summary>
        public async Task<List<AuditLog>> GetAuditLogsAsync(string eventType = null, string userId = null, 
            DateTime? from = null, DateTime? to = null, int page = 1, int pageSize = 50)
        {
            try
            {
                var query = _dbContext.AuditLogs.AsQueryable();
                
                if (!string.IsNullOrEmpty(eventType))
                    query = query.Where(a => a.EventType == eventType);
                
                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(a => a.UserId == userId);
                
                if (from.HasValue)
                    query = query.Where(a => a.Timestamp >= from.Value);
                
                if (to.HasValue)
                    query = query.Where(a => a.Timestamp <= to.Value);
                
                var logs = await query
                    .OrderByDescending(a => a.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                
                return logs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit logs");
                throw;
            }
        }
    }
}
