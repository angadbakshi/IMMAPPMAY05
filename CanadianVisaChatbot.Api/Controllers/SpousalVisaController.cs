using CanadianVisaChatbot.Api.Models;
using CanadianVisaChatbot.Shared.AI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CanadianVisaChatbot.Api.Controllers;

/// <summary>
/// Controller for handling spousal sponsorship visa-related operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SpousalVisaController : ControllerBase
{
    private readonly IVisaProcessingService _visaProcessingService;
    private readonly ILogger<SpousalVisaController> _logger;

    public SpousalVisaController(
        IVisaProcessingService visaProcessingService,
        ILogger<SpousalVisaController> logger)
    {
        _visaProcessingService = visaProcessingService;
        _logger = logger;
    }

    /// <summary>
    /// Assesses the genuineness of a relationship for spousal sponsorship
    /// </summary>
    /// <param name="request">Relationship details including type, duration, living situation, and communication evidence</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Relationship assessment result</returns>
    /// <response code="200">Returns the relationship assessment</response>
    /// <response code="500">If there was an internal error processing the request</response>
    [HttpPost("assess-relationship")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> AssessRelationship(
        SpousalRelationshipRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var relationshipDetails = new Dictionary<string, string>
            {
                ["type"] = request.Type,
                ["duration"] = request.Duration,
                ["livingSituation"] = request.LivingSituation,
                ["communication"] = request.Communication
            };

            var result = await _visaProcessingService.AssessSpousalRelationshipAsync(relationshipDetails, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assessing spousal relationship");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Generates a sponsorship letter for a spousal visa application
    /// </summary>
    /// <param name="request">Sponsor details including name, relationship duration, financial capacity, and living arrangements</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Generated sponsorship letter</returns>
    /// <response code="200">Returns the generated sponsor letter</response>
    /// <response code="500">If there was an internal error processing the request</response>
    [HttpPost("generate-sponsor-letter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> GenerateSponsorLetter(
        SponsorLetterRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var sponsorDetails = new Dictionary<string, string>
            {
                ["name"] = request.Name,
                ["duration"] = request.Duration,
                ["financialCapacity"] = request.FinancialCapacity,
                ["livingArrangements"] = request.LivingArrangements
            };

            var result = await _visaProcessingService.GenerateSponsorLetterAsync(sponsorDetails, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating sponsor letter");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}