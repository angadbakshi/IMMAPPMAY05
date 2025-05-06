using CanadianVisaChatbot.Shared.Models;
using CanadianVisaChatbot.Shared.AI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CanadianVisaChatbot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudyVisaController : ControllerBase
{
    private readonly IVisaProcessingService _visaProcessingService;
    private readonly ILogger<StudyVisaController> _logger;
    private readonly IWebHostEnvironment _environment;

    public StudyVisaController(
        IVisaProcessingService visaProcessingService,
        ILogger<StudyVisaController> logger,
        IWebHostEnvironment environment)
    {
        _visaProcessingService = visaProcessingService;
        _logger = logger;
        _environment = environment;
    }

    [HttpPost("assess-eligibility")]
    public async Task<ActionResult<string>> AssessEligibility(
        StudyVisaEligibilityRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Processing study visa eligibility assessment for nationality: {Nationality}",
                request.Nationality);

            var userData = new Dictionary<string, string>
            {
                ["age"] = request.Age,
                ["nationality"] = request.Nationality,
                ["education"] = request.Education
            };

            var result = await _visaProcessingService.AssessStudyVisaEligibilityAsync(userData, cancellationToken);
            
            _logger.LogInformation("Successfully processed study visa eligibility assessment");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assessing study visa eligibility: {Message}", ex.Message);
            
            if (_environment.IsDevelopment())
            {
                return StatusCode(500, $"Error processing request: {ex.Message}");
            }
            
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpPost("generate-study-plan")]
    public async Task<ActionResult<string>> GenerateStudyPlan(
        StudyPlanRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Generating study plan for program: {Program} at {Institution}",
                request.Program, request.Institution);

            var programDetails = new Dictionary<string, string>
            {
                ["program"] = request.Program,
                ["institution"] = request.Institution,
                ["duration"] = request.Duration,
                ["careerGoals"] = request.CareerGoals
            };

            var result = await _visaProcessingService.GenerateStudyPlanAsync(programDetails, cancellationToken);
            
            _logger.LogInformation("Successfully generated study plan");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating study plan: {Message}", ex.Message);
            
            if (_environment.IsDevelopment())
            {
                return StatusCode(500, $"Error processing request: {ex.Message}");
            }
            
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}