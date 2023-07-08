using System.Security.Cryptography;
using OtpGenerator.Services.Otp.Interfaces;

namespace OtpGenerator.Services.Otp.Dependencies;

public class CustomOtpGenerator : IOtpGenerator
{
    private const int BinaryOtpLength = 4;
    private const int SignificantBitsMask = 0x7FFFFFFF;
    private const int PasswordLength = 6;
    private const byte LastByteMask = 0x0F;

    public string GenerateOtp(byte[] sharedSecret, long timeStep)
    {
        using var hmac = new HMACSHA256(sharedSecret);
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
        return otp.ToString().PadLeft(PasswordLength, '0');;
    }
}