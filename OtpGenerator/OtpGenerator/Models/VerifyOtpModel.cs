using System.ComponentModel.DataAnnotations;

namespace OtpGenerator.Models;

public class VerifyOtpModel
{
    [Required]
    public string UserId { get; set; }
    [Required]
    public string Otp { get; set; }
}