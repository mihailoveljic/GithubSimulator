using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Labels;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
//[Authorize]
[Route("[controller]")]
public class LabelController : ControllerBase
{
	private readonly ILabelService labelService;
	private readonly ILogger<LabelController> logger;
	private readonly LabelFactory labelFactory;
	private readonly ICacheService cacheService;

	public LabelController(
		ILabelService labelService,
		ILogger<LabelController> logger,
		LabelFactory labelFactory,
		ICacheService cacheService)
	{
		this.labelService = labelService;
		this.logger = logger;
		this.labelFactory = labelFactory;
		this.cacheService = cacheService;
	}

    [HttpGet("All", Name = "GetAllLabels")]
    public async Task<IActionResult> GetAllLabels()
    {
        try
        {
            var cachedLabels = await cacheService.GetAllLabelsAsync();

            if (cachedLabels != null && cachedLabels.Any())
            {
                logger.LogInformation("Serving labels from cache");
                return Ok(cachedLabels);
            }

            var labels = await labelService.GetAll();
            if (labels.Any())
            {
                foreach (var label in labels)
                {
                    await cacheService.SetLabelData(label, DateTimeOffset.UtcNow.AddHours(2)); // Assume SetLabelData is implemented
                }
            }
            logger.LogInformation("Serving labels from database");
            return Ok(labels);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching all labels");
            return BadRequest(ex.Message);
        }
    }


    [HttpGet(Name = "GetLabelById")]
	public async Task<IActionResult> GetById([FromQuery] Guid id)
	{
		return (await labelService.GetById(id))
		.Map(pullRequest => (IActionResult)Ok(pullRequest))
		.GetValueOrDefault(() =>
		{
			return NotFound();
		});
	}

    [HttpPost]
    public async Task<IActionResult> CreateLabel([FromBody] InsertLabelDto dto)
    {
        try
        {
            var label = labelFactory.MapToDomain(dto);
            var result = await labelService.Insert(label);

            // Invalidate cache after successful creation
            await cacheService.RemoveAllLabelDataAsync(); // Assume RemoveAllLabelDataAsync is implemented

            logger.LogInformation("Label successfully created");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating label");
            return StatusCode(500, ex.Message);
        }
    }


    [HttpPut]
    public async Task<IActionResult> UpdateLabel([FromBody] UpdateLabelDto dto)
    {
        try
        {
            var labelUpdate = labelFactory.MapToDomain(dto);
            var response = await labelService.Update(labelUpdate);

            if (response.HasValue)
            {
                // Invalidate cache after successful update
                await cacheService.RemoveAllLabelDataAsync();

                logger.LogInformation($"Label {dto.Id} successfully updated");
                return Ok(response.Value);
            }
            else
            {
                return NotFound($"Label with ID {dto.Id} not found");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error updating label {dto.Id}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteLabel(Guid id)
    {
        try
        {
            var response = await labelService.Delete(id);
            if (response)
            {
                await cacheService.RemoveAllLabelDataAsync();

                logger.LogInformation($"Label {id} deleted successfully");
                return Ok($"Label {id} successfully deleted");
            }
            else
            {
                return NotFound($"Label with ID {id} not found");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error deleting label {id}");
            return StatusCode(500, ex.Message);
        }
    }
}
