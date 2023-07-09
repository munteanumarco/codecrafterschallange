namespace OtpGenerator.Constants;

public static class OtpConstants
{
    public const int BinaryOtpLength = 4;
    public const int SignificantBitsMask = 0x7FFFFFFF;
    public const int PasswordLength = 6;
    public const byte LastByteMask = 0x0F;
    public const int ValidityPeriodInSeconds = 30;
}