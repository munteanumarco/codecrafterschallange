using OtpGenerator.Services.Interfaces;

namespace OtpGenerator.Services;

public class SystemClock : IClock
{
    public DateTime Now => DateTime.Now;
}