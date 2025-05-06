using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CanadianVisaChatbot.Shared.Models;

namespace CanadianVisaChatbot.Shared.Services;

public interface IVisaApplicationService
{
    // Application Management
    Task<VisaApplication> CreateApplicationAsync(string userId, VisaType type);
    Task<VisaApplication> GetApplicationAsync(string applicationId);
    Task<List<VisaApplication>> GetUserApplicationsAsync(string userId);
    Task<VisaApplication> UpdateApplicationAsync(VisaApplication application);
    Task DeleteApplicationAsync(string applicationId);

    // Document Management
    Task<DocumentInfo> AddDocumentAsync(string applicationId, DocumentInfo document, Stream fileStream);
    Task<DocumentInfo> UpdateDocumentAsync(string applicationId, DocumentInfo document);
    Task<bool> DeleteDocumentAsync(string applicationId, string documentId);
    Task<Stream> GetDocumentStreamAsync(string applicationId, string documentId);
    Task<List<DocumentInfo>> GetRequiredDocumentsAsync(string applicationId);

    // Step Management
    Task<List<ApplicationStep>> GetApplicationStepsAsync(string applicationId);
    Task<ApplicationStep> UpdateStepStatusAsync(string applicationId, string stepId, StepStatus status);
    Task<ApplicationStep> GetCurrentStepAsync(string applicationId);
    Task<bool> ValidateStepCompletionAsync(string applicationId, string stepId);

    // Status Management
    Task<ApplicationStatus> GetApplicationStatusAsync(string applicationId);
    Task<ApplicationStatus> UpdateApplicationStatusAsync(string applicationId, ApplicationStatus status);
    Task<bool> ValidateApplicationStatusAsync(string applicationId, ApplicationStatus newStatus);

    // Progress Tracking
    Task<double> GetApplicationProgressAsync(string applicationId);
    Task<List<string>> GetPendingRequirementsAsync(string applicationId);
    Task<Dictionary<string, DateTime?>> GetTimelineAsync(string applicationId);

    // AI-Assisted Features
    Task<List<string>> GetDocumentSuggestionsAsync(string applicationId);
    Task<List<string>> ValidateDocumentContentAsync(string applicationId, string documentId);
    Task<string> GenerateNextStepGuidanceAsync(string applicationId);
    Task<Dictionary<string, string>> GetPersonalizedTipsAsync(string applicationId);
}