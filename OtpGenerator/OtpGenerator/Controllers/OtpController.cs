using Microsoft.AspNetCore.Mvc;
using OtpGenerator.Models;
using OtpGenerator.Services.Interfaces;

namespace OtpGenerator.Controllers;

[ApiController]
[Route("api/otp")]
public class OtpController : ControllerBase
{
    private readonly IOtpService _otpService;

    public OtpController(IOtpService otpService)
    {
        _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
    }
    
    [HttpGet]
    public ActionResult GetOneTimePassword(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("User ID cannot be null or empty.");
        }
        var otp = _otpService.GetOtp(userId);
        return Ok(new {code = otp});
    }

    [HttpPost]
    public ActionResult<bool> VerifyOneTimePassword([FromBody] VerifyOtpModel inputData)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        if (_otpService.VerifyOtp(inputData.UserId, inputData.Otp))
        {
            return Ok(true);
        }
        
        return Unauthorized(false);
    }
}