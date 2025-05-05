namespace CanadianVisaChatbot.Shared.AI.Services;

public interface IVisaProcessingService
{
    Task<string> AssessStudyVisaEligibilityAsync(Dictionary<string, string> userData, CancellationToken cancellationToken = default);
    Task<string> GenerateStudyPlanAsync(Dictionary<string, string> programDetails, CancellationToken cancellationToken = default);
    
    Task<string> AssessWorkVisaLMIAAsync(Dictionary<string, string> jobDetails, CancellationToken cancellationToken = default);
    Task<string> GenerateEmploymentLetterAsync(Dictionary<string, string> employmentDetails, CancellationToken cancellationToken = default);
    
    Task<string> AssessSpousalRelationshipAsync(Dictionary<string, string> relationshipDetails, CancellationToken cancellationToken = default);
    Task<string> GenerateSponsorLetterAsync(Dictionary<string, string> sponsorDetails, CancellationToken cancellationToken = default);
}