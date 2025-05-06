using CanadianVisaChatbot.Shared.AI.Extensions;
using CanadianVisaChatbot.Shared.Services;
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // For development, accept any token
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                Console.WriteLine("Running in Development mode - Authentication is disabled");
                return Task.CompletedTask;
            }
        };
    });

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HTTP logging for development
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("Authorization");
    logging.ResponseHeaders.Add("Content-Type");
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Firebase
var firestoreDb = new FirestoreDbBuilder
{
    ProjectId = builder.Configuration["Firebase:ProjectId"],
    JsonCredentials = File.ReadAllText("firebase-credentials.json")
}.Build();

var storage = StorageClient.Create();
builder.Services.AddSingleton(firestoreDb);
builder.Services.AddSingleton(storage);

// Add DeepSeek services
builder.Services.AddVisaServices(
    builder.Configuration["DeepSeek:ApiKey"],
    builder.Configuration["DeepSeek:BaseUrl"]
);

// Register application services
builder.Services.AddTransient<IVisaApplicationService, VisaApplicationService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpLogging();
    Console.WriteLine("Running in Development mode - Authentication is disabled");
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
