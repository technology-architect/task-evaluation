using System.Security.Cryptography;
using System.Text;
using EmployeeMonitoring.Common.Constants;
using EmployeeMonitoring.Common.Models;

namespace EmployeeMonitoring.Infrastructure.Encryption
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText, byte[] key = null);
        string Decrypt(string cipherText, byte[] key = null);
        bool VerifyIntegrity(string data, string hash);
        string ComputeHash(string data);
        byte[] GenerateKey();
    }
    
    public class AesGcmEncryptionService : IEncryptionService
    {
        private readonly byte[] _masterKey;
        private readonly ILogger<AesGcmEncryptionService> _logger;
        
        public AesGcmEncryptionService(IConfiguration configuration, ILogger<AesGcmEncryptionService> logger)
        {
            _logger = logger;
            
            // Get master key from configuration/environment
            var keyString = configuration["Encryption:MasterKey"];
            if (string.IsNullOrEmpty(keyString))
            {
                throw new InvalidOperationException("Encryption:MasterKey not configured");
            }
            
            try
            {
                _masterKey = Convert.FromBase64String(keyString);
                if (_masterKey.Length != MonitoringConstants.EncryptionKeySize)
                {
                    throw new InvalidOperationException(
                        $"Encryption key must be {MonitoringConstants.EncryptionKeySize} bytes");
                }
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("Encryption key is not valid base64", ex);
            }
        }
        
        /// <summary>
        /// Encrypts plaintext using AES-256-GCM
        /// </summary>
        public string Encrypt(string plainText, byte[] key = null)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText))
                    return plainText;
                
                var encryptionKey = key ?? _masterKey;
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                
                using (var aes = new AesGcm(encryptionKey))
                {
                    // Generate random nonce (IV)
                    var nonce = new byte[MonitoringConstants.GcmNonceSize];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(nonce);
                    }
                    
                    // Ciphertext and tag
                    var cipherBytes = new byte[plainBytes.Length];
                    var tag = new byte[MonitoringConstants.GcmTagSize];
                    
                    // Encrypt
                    aes.Encrypt(nonce, plainBytes, cipherBytes, tag);
                    
                    // Combine nonce + ciphertext + tag
                    var encryptedBytes = new byte[nonce.Length + cipherBytes.Length + tag.Length];
                    Buffer.BlockCopy(nonce, 0, encryptedBytes, 0, nonce.Length);
                    Buffer.BlockCopy(cipherBytes, 0, encryptedBytes, nonce.Length, cipherBytes.Length);
                    Buffer.BlockCopy(tag, 0, encryptedBytes, nonce.Length + cipherBytes.Length, tag.Length);
                    
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encryption failed");
                throw;
            }
        }
        
        /// <summary>
        /// Decrypts AES-256-GCM ciphertext
        /// </summary>
        public string Decrypt(string cipherText, byte[] key = null)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText))
                    return cipherText;
                
                var encryptionKey = key ?? _masterKey;
                var encryptedBytes = Convert.FromBase64String(cipherText);
                
                // Extract nonce, ciphertext, and tag
                var nonce = new byte[MonitoringConstants.GcmNonceSize];
                var cipherBytes = new byte[encryptedBytes.Length - nonce.Length - MonitoringConstants.GcmTagSize];
                var tag = new byte[MonitoringConstants.GcmTagSize];
                
                Buffer.BlockCopy(encryptedBytes, 0, nonce, 0, nonce.Length);
                Buffer.BlockCopy(encryptedBytes, nonce.Length, cipherBytes, 0, cipherBytes.Length);
                Buffer.BlockCopy(encryptedBytes, nonce.Length + cipherBytes.Length, tag, 0, tag.Length);
                
                using (var aes = new AesGcm(encryptionKey))
                {
                    var plainBytes = new byte[cipherBytes.Length];
                    aes.Decrypt(nonce, cipherBytes, tag, plainBytes);
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }
            catch (CryptographicException ex)
            {
                _logger.LogError(ex, "Decryption failed - authentication tag verification failed");
                throw new InvalidOperationException("Decryption failed - data may be corrupted or tampered", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Decryption failed");
                throw;
            }
        }
        
        /// <summary>
        /// Verifies data integrity using SHA-256
        /// </summary>
        public bool VerifyIntegrity(string data, string hash)
        {
            var computedHash = ComputeHash(data);
            return computedHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// Computes SHA-256 hash of data
        /// </summary>
        public string ComputeHash(string data)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToHexString(hashedBytes);
            }
        }
        
        /// <summary>
        /// Generates a new random encryption key
        /// </summary>
        public byte[] GenerateKey()
        {
            var key = new byte[MonitoringConstants.EncryptionKeySize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        }
    }
}
