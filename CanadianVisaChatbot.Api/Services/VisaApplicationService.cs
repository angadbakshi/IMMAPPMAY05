using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CanadianVisaChatbot.Shared.Models;
using CanadianVisaChatbot.Shared.Services;
using Google.Cloud.Storage.V1;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;

namespace CanadianVisaChatbot.Api.Services;

public class VisaApplicationService : IVisaApplicationService
{
    private readonly FirestoreDb _firestore;
    private readonly StorageClient _storage;
    private readonly IDeepSeekClient _deepSeekClient;
    private readonly ILogger<VisaApplicationService> _logger;
    private readonly string _bucketName;

    public VisaApplicationService(
        FirestoreDb firestore,
        StorageClient storage,
        IDeepSeekClient deepSeekClient,
        ILogger<VisaApplicationService> logger,
        string bucketName)
    {
        _firestore = firestore;
        _storage = storage;
        _deepSeekClient = deepSeekClient;
        _logger = logger;
        _bucketName = bucketName;
    }

    public async Task<VisaApplication> CreateApplicationAsync(string userId, VisaType type)
    {
        var application = new VisaApplication
        {
            UserId = userId,
            Type = type,
            Status = ApplicationStatus.Initial,
            Steps = await GenerateApplicationStepsAsync(type)
        };

        var docRef = _firestore.Collection("applications").Document(application.Id);
        await docRef.SetAsync(application);

        return application;
    }

    public async Task<VisaApplication> GetApplicationAsync(string applicationId)
    {
        var docRef = _firestore.Collection("applications").Document(applicationId);
        var snapshot = await docRef.GetSnapshotAsync();
        return snapshot.ConvertTo<VisaApplication>();
    }

    public async Task<List<VisaApplication>> GetUserApplicationsAsync(string userId)
    {
        var query = _firestore.Collection("applications").WhereEqualTo("UserId", userId);
        var snapshot = await query.GetSnapshotAsync();
        return snapshot.Documents.Select(doc => doc.ConvertTo<VisaApplication>()).ToList();
    }

    public async Task<VisaApplication> UpdateApplicationAsync(VisaApplication application)
    {
        application.LastModified = DateTime.UtcNow;
        var docRef = _firestore.Collection("applications").Document(application.Id);
        await docRef.SetAsync(application);
        return application;
    }

    public async Task DeleteApplicationAsync(string applicationId)
    {
        // Delete documents from storage
        var application = await GetApplicationAsync(applicationId);
        foreach (var doc in application.Documents)
        {
            await _storage.DeleteObjectAsync(_bucketName, doc.StoragePath);
        }

        // Delete application document
        await _firestore.Collection("applications").Document(applicationId).DeleteAsync();
    }

    public async Task<DocumentInfo> AddDocumentAsync(string applicationId, DocumentInfo document, Stream fileStream)
    {
        var storagePath = $"{applicationId}/{document.Id}/{document.Name}";
        await _storage.UploadObjectAsync(_bucketName, storagePath, null, fileStream);
        
        document.StoragePath = storagePath;
        document.Status = DocumentStatus.Uploaded;
        document.SubmittedDate = DateTime.UtcNow;

        var application = await GetApplicationAsync(applicationId);
        application.Documents.Add(document);
        await UpdateApplicationAsync(application);

        // Validate document content using AI
        var validationMessages = await ValidateDocumentContentAsync(applicationId, document.Id);
        if (validationMessages.Any())
        {
            document.Status = DocumentStatus.NeedsUpdate;
            document.ValidationMessages = validationMessages;
            await UpdateDocumentAsync(applicationId, document);
        }

        return document;
    }

    public async Task<Stream> GetDocumentStreamAsync(string applicationId, string documentId)
    {
        var application = await GetApplicationAsync(applicationId);
        var document = application.Documents.FirstOrDefault(d => d.Id == documentId);
        
        if (document == null) return null;

        var stream = new MemoryStream();
        await _storage.DownloadObjectAsync(_bucketName, document.StoragePath, stream);
        stream.Position = 0;
        return stream;
    }

    public async Task<List<string>> ValidateDocumentContentAsync(string applicationId, string documentId)
    {
        var application = await GetApplicationAsync(applicationId);
        var document = application.Documents.FirstOrDefault(d => d.Id == documentId);
        
        if (document == null) return new List<string>();

        // Use DeepSeek to analyze document content
        var prompt = $"Validate this {document.Type} document for a {application.Type} visa application. Check for completeness and accuracy.";
        var response = await _deepSeekClient.GetResponseAsync(prompt);
        
        return response.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
    }

    private async Task<List<ApplicationStep>> GenerateApplicationStepsAsync(VisaType type)
    {
        // Use DeepSeek to generate customized steps
        var prompt = $"Generate a detailed step-by-step guide for {type} visa application process";
        var response = await _deepSeekClient.GetResponseAsync(prompt);
        
        var steps = response.Split('\n')
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select((step, index) => new ApplicationStep
            {
                Name = step,
                Order = index + 1,
                Status = index == 0 ? StepStatus.InProgress : StepStatus.NotStarted
            })
            .ToList();

        return steps;
    }

    // Implement remaining interface methods...
    public async Task<double> GetApplicationProgressAsync(string applicationId)
    {
        var application = await GetApplicationAsync(applicationId);
        var totalSteps = application.Steps.Count;
        var completedSteps = application.Steps.Count(s => s.Status == StepStatus.Completed);
        return (double)completedSteps / totalSteps * 100;
    }

    public async Task<List<string>> GetPendingRequirementsAsync(string applicationId)
    {
        var application = await GetApplicationAsync(applicationId);
        var currentStep = application.Steps.FirstOrDefault(s => s.Status == StepStatus.InProgress);
        return currentStep?.Requirements ?? new List<string>();
    }

    public async Task<Dictionary<string, DateTime?>> GetTimelineAsync(string applicationId)
    {
        var application = await GetApplicationAsync(applicationId);
        return application.Steps
            .OrderBy(s => s.Order)
            .ToDictionary(s => s.Name, s => s.CompletedDate);
    }
}