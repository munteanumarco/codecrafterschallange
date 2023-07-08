using OtpGenerator.Services.Interfaces;
namespace OtpGenerator.Services;
using System.Security.Cryptography;

public class OtpService : IOtpService
{
    private const int ValidityPeriod = 30;
    private const int PasswordLength = 6;
    private const byte LastByteMask = 0x0F;
    private const int SignificantBitsMask = 0x7FFFFFFF;
    private const int BinaryOtpLength = 4;

    private readonly IClock _clock;
    
    public OtpService(IClock clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }
    
    public string GetOtp(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
        }

        var sharedSecret = GetSharedSecret(userId);
        var timeStep = GetTimeStep(_clock.Now);
        var otp = GenerateOtp(sharedSecret, timeStep);
        
        return otp.PadLeft(PasswordLength, '0');
    }   
    
    public bool VerifyOtp(string userId, string otp)
    {   
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
        }
        
        var calculatedOtp = GetOtp(userId);
        return calculatedOtp == otp;
    }

    private static byte[] GetSharedSecret(string userId)
    {
        using var sha256 = SHA256.Create();
        var userIdBytes = System.Text.Encoding.UTF8.GetBytes(userId);
        return sha256.ComputeHash(userIdBytes);
    }

    private static long GetTimeStep(DateTime dateTime)
    {
        dateTime = dateTime.AddSeconds(-(dateTime.Second % ValidityPeriod));
        var unixTimestamp = (long)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        var timeStep = unixTimestamp / ValidityPeriod;
        
        return timeStep;
    }
    
    private static string GenerateOtp(byte[] secret, long timeStep)
    {
        using var hmac = new HMACSHA1(secret);
        var timeStepBytes = BitConverter.GetBytes(timeStep);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timeStepBytes);
        }
        var hash = hmac.ComputeHash(timeStepBytes);
        var offset = hash[hash.Length - 1] & LastByteMask;
        var binaryOtp = new byte[BinaryOtpLength];
        Array.Copy(hash, offset, binaryOtp, 0, BinaryOtpLength);
            
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(binaryOtp);
        }
        var otp = BitConverter.ToInt32(binaryOtp, 0);
        otp &= SignificantBitsMask;
        otp = Math.Abs(otp) % (int)Math.Pow(10, PasswordLength);
        return otp.ToString();
    }
}