using System;
using System.Collections.Generic;

namespace CanadianVisaChatbot.Shared.Models;

public class VisaApplication
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public VisaType Type { get; set; }
    public ApplicationStatus Status { get; set; }
    public List<DocumentInfo> Documents { get; set; } = new();
    public Dictionary<string, string> PersonalInfo { get; set; } = new();
    public List<ApplicationStep> Steps { get; set; } = new();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}

public enum VisaType
{
    Study,
    Work,
    Spousal
}

public static class VisaTypeExtensions
{
    public static VisaType[] Values => Enum.GetValues<VisaType>();
}

public enum ApplicationStatus
{
    Initial,
    DocumentGathering,
    DocumentsComplete,
    InReview,
    AdditionalDocumentsRequested,
    ReadyForSubmission,
    Submitted,
    InProcessing,
    Approved,
    Rejected
}

public class DocumentInfo
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public DocumentType Type { get; set; }
    public DocumentStatus Status { get; set; }
    public string StoragePath { get; set; }
    public DateTime? SubmittedDate { get; set; }
    public string Notes { get; set; }
    public List<string> ValidationMessages { get; set; } = new();
}

public enum DocumentType
{
    Passport,
    Photo,
    EducationCredentials,
    LanguageTest,
    FinancialProof,
    MedicalExam,
    PoliceCheck,
    EmploymentLetter,
    MarriageCertificate,
    LMIA,
    StudyPermit,
    AdmissionLetter,
    Other
}

public static class DocumentTypeExtensions
{
    public static DocumentType[] Values => Enum.GetValues<DocumentType>();
}

public enum DocumentStatus
{
    Required,
    Missing,
    Uploaded,
    InReview,
    Accepted,
    Rejected,
    NeedsUpdate
}

public class ApplicationStep
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Description { get; set; }
    public StepStatus Status { get; set; }
    public int Order { get; set; }
    public DateTime? CompletedDate { get; set; }
    public List<string> Requirements { get; set; } = new();
    public List<string> Instructions { get; set; } = new();
}

public enum StepStatus
{
    NotStarted,
    InProgress,
    Completed,
    Blocked,
    NeedsAttention
}