using OtpGenerator.Services.Otp.Interfaces;

namespace OtpGenerator.Services.Otp;

public class OtpService : IOtpService
{
    private const int ValidityPeriodInSeconds = 30;
    
    private readonly IOtpGenerator _otpGenerator;
    private readonly ISharedSecretProvider _sharedSecretProvider;
    private readonly ITimeProvider _timeProvider;
    
    public OtpService(IOtpGenerator otpGenerator, ISharedSecretProvider sharedSecretProvider, ITimeProvider timeProvider)
    {
        _otpGenerator = otpGenerator ?? throw new ArgumentNullException(nameof(otpGenerator));
        _sharedSecretProvider = sharedSecretProvider ?? throw new ArgumentNullException(nameof(sharedSecretProvider));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }
    
    public string GetOtp(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
        }

        var sharedSecret = _sharedSecretProvider.GetSharedSecret(userId);
        var timeStep = GetTimeStep(_timeProvider.GetCurrentTime());
        var otp = _otpGenerator.GenerateOtp(sharedSecret, timeStep);

        return otp;
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

    private static long GetTimeStep(DateTime dateTime)
    {
        dateTime = dateTime.AddSeconds(-(dateTime.Second % ValidityPeriodInSeconds));
        var unixTimestamp = (long)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        var timeStep = unixTimestamp / ValidityPeriodInSeconds;
        
        return timeStep;
    }
}