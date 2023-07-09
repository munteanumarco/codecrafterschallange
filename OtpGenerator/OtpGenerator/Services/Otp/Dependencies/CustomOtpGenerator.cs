using System.Security.Cryptography;
using OtpGenerator.Constants;
using OtpGenerator.Services.Otp.Interfaces;

namespace OtpGenerator.Services.Otp.Dependencies;

public class CustomOtpGenerator : IOtpGenerator
{
    public string GenerateOtp(byte[] sharedSecret, long timeStep)
    {
        using var hmac = new HMACSHA256(sharedSecret);
        var timeStepBytes = BitConverter.GetBytes(timeStep);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timeStepBytes);
        }
        var hash = hmac.ComputeHash(timeStepBytes);
        var offset = hash[hash.Length - 1] & OtpConstants.LastByteMask;
        var binaryOtp = new byte[OtpConstants.BinaryOtpLength];
        Array.Copy(hash, offset, binaryOtp, 0, OtpConstants.BinaryOtpLength);

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(binaryOtp);
        }

        var otp = BitConverter.ToInt32(binaryOtp, 0);
        otp &= OtpConstants.SignificantBitsMask;
        otp = Math.Abs(otp) % (int)Math.Pow(10, OtpConstants.PasswordLength);
        return otp.ToString().PadLeft(OtpConstants.PasswordLength, '0');;
    }
}