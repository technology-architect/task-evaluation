using Microsoft.EntityFrameworkCore;
using EmployeeMonitoring.Common.Models;
using EmployeeMonitoring.Services.Interfaces;
using EmployeeMonitoring.Infrastructure.Data;

namespace EmployeeMonitoring.Services.Implementations
{
    public class ConsentService : IConsentService
    {
        private readonly MonitoringDbContext _dbContext;
        private readonly ILogger<ConsentService> _logger;
        private readonly IAuditService _auditService;
        
        public ConsentService(MonitoringDbContext dbContext, ILogger<ConsentService> logger, IAuditService auditService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _auditService = auditService;
        }
        
        /// <summary>
        /// Gets the current consent status for an employee
        /// </summary>
        public async Task<EmployeeConsent> GetConsentAsync(int employeeId)
        {
            var consent = await _dbContext.EmployeeConsents
                .Where(c => c.EmployeeId == employeeId && c.RevokedDate == null)
                .OrderByDescending(c => c.ConsentDate)
                .FirstOrDefaultAsync();
            
            return consent;
        }
        
        /// <summary>
        /// Requests consent from an employee
        /// </summary>
        public async Task<ConsentRequest> RequestConsentAsync(int employeeId, bool keystrokeCapture, 
            bool screenshotCapture, string reason)
        {
            try
            {
                var consentRequest = new ConsentRequest
                {
                    EmployeeId = employeeId,
                    KeystrokeCapture = keystrokeCapture,
                    ScreenshotCapture = screenshotCapture,
                    Reason = reason,
                    Status = ConsentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };
                
                _dbContext.ConsentRequests.Add(consentRequest);
                await _dbContext.SaveChangesAsync();
                
                _logger.LogInformation(
                    $"Consent request created for employee {employeeId}: Keystroke={keystrokeCapture}, Screenshot={screenshotCapture}");
                
                return consentRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating consent request for employee {employeeId}");
                throw;
            }
        }
        
        /// <summary>
        /// Grants consent for monitoring
        /// </summary>
        public async Task<bool> GrantConsentAsync(int employeeId, bool keystrokeConsent, bool screenshotConsent)
        {
            try
            {
                // Revoke previous consent if exists
                var existingConsent = await _dbContext.EmployeeConsents
                    .Where(c => c.EmployeeId == employeeId && c.RevokedDate == null)
                    .FirstOrDefaultAsync();
                
                if (existingConsent != null)
                {
                    existingConsent.RevokedDate = DateTime.UtcNow;
                }
                
                // Create new consent record
                var consent = new EmployeeConsent
                {
                    EmployeeId = employeeId,
                    KeystrokeConsentGiven = keystrokeConsent,
                    ScreenshotConsentGiven = screenshotConsent,
                    ConsentDate = DateTime.UtcNow,
                    ConsentVersion = "1.0",
                    ConsentHash = GenerateConsentHash(employeeId, keystrokeConsent, screenshotConsent)
                };
                
                _dbContext.EmployeeConsents.Add(consent);
                await _dbContext.SaveChangesAsync();
                
                _logger.LogInformation(
                    $"Consent granted for employee {employeeId}: Keystroke={keystrokeConsent}, Screenshot={screenshotConsent}");
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error granting consent for employee {employeeId}");
                throw;
            }
        }
        
        /// <summary>
        /// Revokes consent for monitoring
        /// </summary>
        public async Task<bool> RevokeConsentAsync(int employeeId)
        {
            try
            {
                var consent = await _dbContext.EmployeeConsents
                    .Where(c => c.EmployeeId == employeeId && c.RevokedDate == null)
                    .FirstOrDefaultAsync();
                
                if (consent != null)
                {
                    consent.RevokedDate = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();
                    
                    _logger.LogInformation($"Consent revoked for employee {employeeId}");
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error revoking consent for employee {employeeId}");
                throw;
            }
        }
        
        /// <summary>
        /// Checks if employee has valid consent for monitoring type
        /// </summary>
        public async Task<bool> HasValidConsentAsync(int employeeId, string captureType)
        {
            var consent = await GetConsentAsync(employeeId);
            
            if (consent == null || consent.RevokedDate != null)
                return false;
            
            return captureType.ToLower() switch
            {
                "keystroke" => consent.KeystrokeConsentGiven,
                "screenshot" => consent.ScreenshotConsentGiven,
                _ => false
            };
        }
        
        private string GenerateConsentHash(int employeeId, bool keystroke, bool screenshot)
        {
            var data = $"{employeeId}_{keystroke}_{screenshot}_{DateTime.UtcNow:O}";
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
