using System.ComponentModel.DataAnnotations;

namespace OtpGenerator.Models;

public class OtpReturnModel
{
    [Required]
    public string Code { get; set; }

    public OtpReturnModel(string code)
    {
        Code = code;
    }
}