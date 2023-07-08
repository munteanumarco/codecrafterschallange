using Moq;
using OtpGenerator.Services;
using OtpGenerator.Services.Otp;
using OtpGenerator.Services.Otp.Interfaces;

namespace OtpGeneratorTests;

[TestFixture]
public class OtpServiceTests
{
    private OtpService _otpService;
    private Mock<IOtpGenerator> _otpGeneratorMock;
    private Mock<ISharedSecretProvider> _sharedSecretProviderMock;
    private Mock<ITimeProvider> _timeProviderMock;

    [SetUp]
    public void Setup()
    {
        _otpGeneratorMock = new Mock<IOtpGenerator>();
        _sharedSecretProviderMock = new Mock<ISharedSecretProvider>();
        _timeProviderMock = new Mock<ITimeProvider>();

        _otpService = new OtpService(_otpGeneratorMock.Object, _sharedSecretProviderMock.Object, _timeProviderMock.Object);
    }
    
    [Test]
    public void GetOtp_ValidUserId_ReturnsOtp()
    {
        string userId = "testUser";
        string expectedOtp = "123456";
        byte[] sharedSecretBytes = System.Text.Encoding.UTF8.GetBytes("sharedSecret");

        _sharedSecretProviderMock.Setup(provider => provider.GetSharedSecret(userId)).Returns(sharedSecretBytes);
        _timeProviderMock.Setup(provider => provider.GetCurrentTime()).Returns(new DateTime(2023, 1, 1, 10, 0, 0));
        _otpGeneratorMock.Setup(generator => generator.GenerateOtp(sharedSecretBytes, It.IsAny<long>())).Returns(expectedOtp);

        string result = _otpService.GetOtp(userId);

        Assert.AreEqual(expectedOtp, result);
    }
    
    [Test]
    public void GetOtp_EmptyUserId_ThrowsArgumentException()
    {
        string userId = string.Empty;

        Assert.Throws<ArgumentException>(() => _otpService.GetOtp(userId));
    }
    
    [Test]
    public void VerifyOtp_ValidUserIdAndOtp_ReturnsTrue()
    {
        string userId = "testUser";
        string otp = "123456";

        _otpGeneratorMock.Setup(generator => generator.GenerateOtp(It.IsAny<byte[]>(), It.IsAny<long>())).Returns(otp);

        bool result = _otpService.VerifyOtp(userId, otp);

        Assert.IsTrue(result);
    }
    
    [Test]
    public void VerifyOtp_EmptyUserId_ThrowsArgumentException()
    {
        string userId = string.Empty;
        string otp = "123456";

        Assert.Throws<ArgumentException>(() => _otpService.VerifyOtp(userId, otp));
    }

}