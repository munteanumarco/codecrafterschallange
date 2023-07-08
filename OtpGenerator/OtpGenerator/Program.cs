using OtpGenerator.Services;
using OtpGenerator.Services.Otp;
using OtpGenerator.Services.Otp.Dependencies;
using OtpGenerator.Services.Otp.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://codecrafterschallange.netlify.app")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<ITimeProvider, TimeProvider>();
builder.Services.AddScoped<IOtpGenerator, CustomOtpGenerator>();
builder.Services.AddScoped<ISharedSecretProvider, SharedSecretProvider>();
builder.Services.AddScoped<IOtpService, OtpService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();