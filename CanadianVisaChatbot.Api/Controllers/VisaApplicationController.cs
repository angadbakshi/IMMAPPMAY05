using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CanadianVisaChatbot.Shared.Models;
using CanadianVisaChatbot.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CanadianVisaChatbot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VisaApplicationController : ControllerBase
{
    private readonly IVisaApplicationService _applicationService;
    private readonly ILogger<VisaApplicationController> _logger;

    public VisaApplicationController(
        IVisaApplicationService applicationService,
        ILogger<VisaApplicationController> logger)
    {
        _applicationService = applicationService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<VisaApplication>> CreateApplication([FromBody] VisaType type)
    {
        try
        {
            var userId = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var application = await _applicationService.CreateApplicationAsync(userId, type);
            return Ok(application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating visa application");
            return StatusCode(500, "Error creating visa application. Please try again.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VisaApplication>> GetApplication(string id)
    {
        try
        {
            var userId = User.FindFirst("user_id")?.Value;
            var application = await _applicationService.GetApplicationAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            if (application.UserId != userId)
            {
                return Forbid();
            }

            return Ok(application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving visa application {Id}", id);
            return StatusCode(500, "Error retrieving visa application. Please try again.");
        }
    }

    [HttpGet("user")]
    public async Task<ActionResult<List<VisaApplication>>> GetUserApplications()
    {
        try
        {
            var userId = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var applications = await _applicationService.GetUserApplicationsAsync(userId);
            return Ok(applications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user applications");
            return StatusCode(500, "Error retrieving applications. Please try again.");
        }
    }

    [HttpPost("{id}/documents")]
    public async Task<ActionResult<DocumentInfo>> UploadDocument(string id, [FromForm] DocumentUploadRequest request)
    {
        try
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            var document = new DocumentInfo
            {
                Name = request.Name,
                Type = request.Type,
                Status = DocumentStatus.Uploaded,
                Notes = request.Notes
            };

            using var stream = request.File.OpenReadStream();
            var result = await _applicationService.AddDocumentAsync(id, document, stream);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading document for application {Id}", id);
            return StatusCode(500, "Error uploading document. Please try again.");
        }
    }

    [HttpGet("{id}/documents/{documentId}")]
    public async Task<ActionResult> DownloadDocument(string id, string documentId)
    {
        try
        {
            var stream = await _applicationService.GetDocumentStreamAsync(id, documentId);
            if (stream == null)
            {
                return NotFound();
            }

            return File(stream, "application/octet-stream");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading document {DocumentId} for application {Id}", documentId, id);
            return StatusCode(500, "Error downloading document. Please try again.");
        }
    }

    [HttpGet("{id}/progress")]
    public async Task<ActionResult<ApplicationProgress>> GetProgress(string id)
    {
        try
        {
            var progress = await _applicationService.GetApplicationProgressAsync(id);
            var pendingRequirements = await _applicationService.GetPendingRequirementsAsync(id);
            var timeline = await _applicationService.GetTimelineAsync(id);
            var currentStep = await _applicationService.GetCurrentStepAsync(id);

            return Ok(new ApplicationProgress
            {
                Percentage = progress,
                PendingRequirements = pendingRequirements,
                Timeline = timeline,
                CurrentStep = currentStep
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving progress for application {Id}", id);
            return StatusCode(500, "Error retrieving progress. Please try again.");
        }
    }
}

public class DocumentUploadRequest
{
    public string Name { get; set; }
    public DocumentType Type { get; set; }
    public string Notes { get; set; }
    public IFormFile File { get; set; }
}

public class ApplicationProgress
{
    public double Percentage { get; set; }
    public List<string> PendingRequirements { get; set; }
    public Dictionary<string, DateTime?> Timeline { get; set; }
    public ApplicationStep CurrentStep { get; set; }
}