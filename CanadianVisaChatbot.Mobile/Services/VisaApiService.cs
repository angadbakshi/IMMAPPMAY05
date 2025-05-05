using System.Net.Http.Json;
using CanadianVisaChatbot.Api.Models;

namespace CanadianVisaChatbot.Mobile.Services;

public class VisaApiService : IVisaApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<VisaApiService> _logger;

    public VisaApiService(HttpClient httpClient, ILogger<VisaApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> AssessStudyVisaEligibilityAsync(Dictionary<string, string> userData)
    {
        try
        {
            var request = new StudyVisaEligibilityRequest(
                userData.GetValueOrDefault("age", string.Empty),
                userData.GetValueOrDefault("nationality", string.Empty),
                userData.GetValueOrDefault("education", string.Empty)
            );

            var response = await _httpClient.PostAsJsonAsync("api/StudyVisa/assess-eligibility", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assessing study visa eligibility");
            throw;
        }
    }

    public async Task<string> GenerateStudyPlanAsync(Dictionary<string, string> programDetails)
    {
        try
        {
            var request = new StudyPlanRequest(
                programDetails.GetValueOrDefault("program", string.Empty),
                programDetails.GetValueOrDefault("institution", string.Empty),
                programDetails.GetValueOrDefault("duration", string.Empty),
                programDetails.GetValueOrDefault("careerGoals", string.Empty)
            );

            var response = await _httpClient.PostAsJsonAsync("api/StudyVisa/generate-study-plan", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating study plan");
            throw;
        }
    }

    public async Task<string> AssessWorkVisaLMIAAsync(Dictionary<string, string> jobDetails)
    {
        try
        {
            var request = new WorkVisaLMIARequest(
                jobDetails.GetValueOrDefault("jobTitle", string.Empty),
                jobDetails.GetValueOrDefault("industry", string.Empty),
                jobDetails.GetValueOrDefault("location", string.Empty),
                jobDetails.GetValueOrDefault("salary", string.Empty)
            );

            var response = await _httpClient.PostAsJsonAsync("api/WorkVisa/assess-lmia", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assessing LMIA requirements");
            throw;
        }
    }

    public async Task<string> GenerateEmploymentLetterAsync(Dictionary<string, string> employmentDetails)
    {
        try
        {
            var request = new EmploymentLetterRequest(
                employmentDetails.GetValueOrDefault("company", string.Empty),
                employmentDetails.GetValueOrDefault("position", string.Empty),
                employmentDetails.GetValueOrDefault("startDate", string.Empty),
                employmentDetails.GetValueOrDefault("terms", string.Empty)
            );

            var response = await _httpClient.PostAsJsonAsync("api/WorkVisa/generate-employment-letter", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating employment letter");
            throw;
        }
    }

    public async Task<string> AssessSpousalRelationshipAsync(Dictionary<string, string> relationshipDetails)
    {
        try
        {
            var request = new SpousalRelationshipRequest(
                relationshipDetails.GetValueOrDefault("type", string.Empty),
                relationshipDetails.GetValueOrDefault("duration", string.Empty),
                relationshipDetails.GetValueOrDefault("livingSituation", string.Empty),
                relationshipDetails.GetValueOrDefault("communication", string.Empty)
            );

            var response = await _httpClient.PostAsJsonAsync("api/SpousalVisa/assess-relationship", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assessing spousal relationship");
            throw;
        }
    }

    public async Task<string> GenerateSponsorLetterAsync(Dictionary<string, string> sponsorDetails)
    {
        try
        {
            var request = new SponsorLetterRequest(
                sponsorDetails.GetValueOrDefault("name", string.Empty),
                sponsorDetails.GetValueOrDefault("duration", string.Empty),
                sponsorDetails.GetValueOrDefault("financialCapacity", string.Empty),
                sponsorDetails.GetValueOrDefault("livingArrangements", string.Empty)
            );

            var response = await _httpClient.PostAsJsonAsync("api/SpousalVisa/generate-sponsor-letter", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating sponsor letter");
            throw;
        }
    }
}