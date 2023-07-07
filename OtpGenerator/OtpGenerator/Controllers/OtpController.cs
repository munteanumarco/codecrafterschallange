using Microsoft.AspNetCore.Mvc;
using OtpGenerator.Models;
using System.Security.Cryptography;

namespace OtpGenerator.Controllers;

[ApiController]
[Route("api/otp")]
public class OtpController : ControllerBase
{
    private const int ValidityPeriod = 30;
    private const int PasswordLength = 6;

    [HttpGet]
    public ActionResult GetOneTimePassword(string userId)
    {
        var otp = CalculateOtp(userId);
        return Ok(otp);
    }

    [HttpPost]
    public ActionResult<bool> VerifyOneTimePassword(VerifyOtpModel inputData)
    {
        var otp = CalculateOtp(inputData.UserId);
        if (otp == inputData.Otp)
        {
            return Ok(true);
        }
        return Unauthorized(false);
    }
    
    private static string CalculateOtp(string userId)
    {
        var sharedSecret = GetSharedSecret(userId);
        var timeStep = GetTimeStep(DateTime.Now);
        var otp = GenerateOtp(sharedSecret, timeStep);
        
        return otp.ToString().PadLeft(PasswordLength, '0');
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
    
    private static int GenerateOtp(byte[] secret, long timeStep)
    {
        using var hmac = new HMACSHA1(secret);
        var timeStepBytes = BitConverter.GetBytes(timeStep);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timeStepBytes);
        }
        var hash = hmac.ComputeHash(timeStepBytes);
        var offset = hash[hash.Length - 1] & 0x0F;
        var binaryOtp = new byte[4];
        Array.Copy(hash, offset, binaryOtp, 0, 4);
            
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(binaryOtp);
        }
        var otp = BitConverter.ToInt32(binaryOtp, 0);
        otp = otp & 0x7FFFFFFF;
        otp = Math.Abs(otp) % (int)Math.Pow(10, PasswordLength);
        return otp;
    }
}