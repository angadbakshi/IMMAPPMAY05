using CanadianVisaChatbot.Shared.Models;
using CanadianVisaChatbot.Shared.AI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CanadianVisaChatbot.Api.Controllers;

/// <summary>
/// Controller for handling work visa-related operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WorkVisaController : ControllerBase
{
    private readonly IVisaProcessingService _visaProcessingService;
    private readonly ILogger<WorkVisaController> _logger;

    public WorkVisaController(
        IVisaProcessingService visaProcessingService,
        ILogger<WorkVisaController> logger)
    {
        _visaProcessingService = visaProcessingService;
        _logger = logger;
    }

    /// <summary>
    /// Assesses Labour Market Impact Assessment (LMIA) requirements for a work visa
    /// </summary>
    /// <param name="request">Job details including title, industry, location, and salary</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>LMIA assessment result</returns>
    /// <response code="200">Returns the LMIA assessment</response>
    /// <response code="500">If there was an internal error processing the request</response>
    [HttpPost("assess-lmia")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> AssessLMIA(
        WorkVisaLMIARequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var jobDetails = new Dictionary<string, string>
            {
                ["jobTitle"] = request.JobTitle,
                ["industry"] = request.Industry,
                ["location"] = request.Location,
                ["salary"] = request.Salary
            };

            var result = await _visaProcessingService.AssessWorkVisaLMIAAsync(jobDetails, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assessing LMIA requirements");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Generates an employment offer letter for a work visa application
    /// </summary>
    /// <param name="request">Employment details including company, position, start date, and terms</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Generated employment offer letter</returns>
    /// <response code="200">Returns the generated employment letter</response>
    /// <response code="500">If there was an internal error processing the request</response>
    [HttpPost("generate-employment-letter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> GenerateEmploymentLetter(
        EmploymentLetterRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var employmentDetails = new Dictionary<string, string>
            {
                ["company"] = request.Company,
                ["position"] = request.Position,
                ["startDate"] = request.StartDate,
                ["terms"] = request.Terms
            };

            var result = await _visaProcessingService.GenerateEmploymentLetterAsync(employmentDetails, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating employment letter");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}