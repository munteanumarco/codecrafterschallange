using Microsoft.AspNetCore.Mvc;
using Moq;
using OtpGenerator.Controllers;
using OtpGenerator.Models;
using OtpGenerator.Services.Otp.Interfaces;

namespace OtpGeneratorTests;

[TestFixture]
public class OtpControllerTests
{
    private OtpController _otpController;
    private Mock<IOtpService> _otpServiceMock;
    
    [SetUp]
    public void Setup()
    {
        _otpServiceMock = new Mock<IOtpService>();
        _otpController = new OtpController(_otpServiceMock.Object);
    }

    [Test]
    public void GetOneTimePassword_WithValidUserId_ReturnsOkResult()
    {
        string userId = "testUser";
        string expectedOtp = "123456";
        _otpServiceMock.Setup(service => service.GetOtp(userId)).Returns(expectedOtp);
        
        var result = _otpController.GetOneTimePassword(userId);
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        var value = (OtpReturnModel)okResult.Value;
        Assert.AreEqual(expectedOtp, value.Code);
    }
    
    [Test]
    public void GetOneTimePassword_WithNullUserId_ReturnsBadRequest()
    {
        string userId = null;
            
        var result = _otpController.GetOneTimePassword(userId);
            
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }
        
    [Test]
    public void GetOneTimePassword_WithEmptyUserId_ReturnsBadRequest()
    {
        string userId = "";
            
        var result = _otpController.GetOneTimePassword(userId);
            
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

}