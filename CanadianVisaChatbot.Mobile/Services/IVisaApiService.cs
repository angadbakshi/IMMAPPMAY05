namespace CanadianVisaChatbot.Mobile.Services;

public interface IVisaApiService
{
    Task<string> AssessStudyVisaEligibilityAsync(Dictionary<string, string> userData);
    Task<string> GenerateStudyPlanAsync(Dictionary<string, string> programDetails);
    
    Task<string> AssessWorkVisaLMIAAsync(Dictionary<string, string> jobDetails);
    Task<string> GenerateEmploymentLetterAsync(Dictionary<string, string> employmentDetails);
    
    Task<string> AssessSpousalRelationshipAsync(Dictionary<string, string> relationshipDetails);
    Task<string> GenerateSponsorLetterAsync(Dictionary<string, string> sponsorDetails);
}