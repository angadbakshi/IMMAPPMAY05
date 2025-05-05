using CanadianVisaChatbot.Shared.AI.Extensions;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddDebug();
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
}

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Canadian Visa Chatbot API", Version = "v1" });
});

// Add CORS for development
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add DeepSeek services
builder.Services.AddVisaServices(
    builder.Configuration["DeepSeek:ApiKey"] ?? throw new InvalidOperationException("DeepSeek API key not found"),
    builder.Configuration["DeepSeek:BaseUrl"] ?? throw new InvalidOperationException("DeepSeek base URL not found")
);

// Initialize Firebase Admin
if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("Running in Development mode - Authentication is disabled");
}
else 
{
    try
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            var projectId = builder.Configuration["Firebase:ProjectId"] ?? 
                throw new InvalidOperationException("Firebase project ID not found in configuration");

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.GetApplicationDefault(),
                ProjectId = projectId
            });

            // Add JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://securetoken.google.com/{projectId}";
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = $"https://securetoken.google.com/{projectId}",
                        ValidateAudience = true,
                        ValidAudience = projectId,
                        ValidateLifetime = true
                    };
                });
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Firebase initialization failed: {ex.Message}");
        throw; // In production, we want to fail if Firebase init fails
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}

app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    app.UseAuthentication();
    app.UseAuthorization();
}

// Global exception handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error != null)
        {
            var ex = error.Error;
            await context.Response.WriteAsJsonAsync(new 
            {
                Message = app.Environment.IsDevelopment() ? ex.Message : "An error occurred processing your request.",
                Detail = app.Environment.IsDevelopment() ? ex.ToString() : null
            });
        }
    });
});

app.MapControllers();

app.Run();
