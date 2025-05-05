using CanadianVisaChatbot.Shared.AI.Models;
using Microsoft.Extensions.Logging;

namespace CanadianVisaChatbot.Shared.AI.Services;

public class VisaProcessingService : IVisaProcessingService
{
    private readonly IDeepSeekClient _deepSeekClient;
    private readonly ILogger<VisaProcessingService> _logger;

    public VisaProcessingService(IDeepSeekClient deepSeekClient, ILogger<VisaProcessingService> logger)
    {
        _deepSeekClient = deepSeekClient;
        _logger = logger;
    }

    public async Task<string> AssessStudyVisaEligibilityAsync(
        Dictionary<string, string> userData, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Assessing study visa eligibility for user");
            ValidateUserData(userData, "age", "nationality", "education");

            var prompt = VisaPrompts.StudyVisa.AssessEligibility(userData);
            return await _deepSeekClient.GetResponseAsync(prompt, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during study visa eligibility assessment");
            throw new Exception("Failed to assess study visa eligibility. Please try again later.", ex);
        }
    }

    public async Task<string> GenerateStudyPlanAsync(
        Dictionary<string, string> programDetails, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating study plan with provided program details");
            ValidateUserData(programDetails, "program", "institution", "duration", "careerGoals");

            var prompt = VisaPrompts.StudyVisa.GenerateStudyPlan(programDetails);
            return await _deepSeekClient.GenerateDocumentAsync(prompt, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating study plan");
            throw new Exception("Failed to generate study plan. Please try again later.", ex);
        }
    }

    public async Task<string> AssessWorkVisaLMIAAsync(
        Dictionary<string, string> jobDetails, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Assessing LMIA requirements for work visa");
            ValidateUserData(jobDetails, "jobTitle", "industry", "location", "salary");

            var prompt = VisaPrompts.WorkVisa.AssessLMIA(jobDetails);
            return await _deepSeekClient.GetResponseAsync(prompt, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during LMIA assessment");
            throw new Exception("Failed to assess LMIA requirements. Please try again later.", ex);
        }
    }

    public async Task<string> GenerateEmploymentLetterAsync(
        Dictionary<string, string> employmentDetails, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating employment letter template");
            ValidateUserData(employmentDetails, "company", "position", "startDate", "terms");

            var prompt = VisaPrompts.WorkVisa.GenerateEmploymentLetter(employmentDetails);
            return await _deepSeekClient.GenerateDocumentAsync(prompt, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating employment letter");
            throw new Exception("Failed to generate employment letter. Please try again later.", ex);
        }
    }

    public async Task<string> AssessSpousalRelationshipAsync(
        Dictionary<string, string> relationshipDetails, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Assessing spousal relationship details");
            ValidateUserData(relationshipDetails, "type", "duration", "livingSituation", "communication");

            var prompt = VisaPrompts.SpousalVisa.AssessRelationship(relationshipDetails);
            return await _deepSeekClient.GetResponseAsync(prompt, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during spousal relationship assessment");
            throw new Exception("Failed to assess spousal relationship. Please try again later.", ex);
        }
    }

    public async Task<string> GenerateSponsorLetterAsync(
        Dictionary<string, string> sponsorDetails, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating sponsor letter template");
            ValidateUserData(sponsorDetails, "name", "duration", "financialCapacity", "livingArrangements");

            var prompt = VisaPrompts.SpousalVisa.GenerateSponsorLetter(sponsorDetails);
            return await _deepSeekClient.GenerateDocumentAsync(prompt, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating sponsor letter");
            throw new Exception("Failed to generate sponsor letter. Please try again later.", ex);
        }
    }

    private void ValidateUserData(Dictionary<string, string> data, params string[] requiredFields)
    {
        var missingFields = requiredFields.Where(field => !data.ContainsKey(field) || string.IsNullOrWhiteSpace(data[field]));
        
        if (missingFields.Any())
        {
            var fields = string.Join(", ", missingFields);
            throw new ArgumentException($"Missing required fields: {fields}");
        }
    }
}