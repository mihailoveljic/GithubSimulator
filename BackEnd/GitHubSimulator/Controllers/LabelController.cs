using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Labels;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
[Route("[controller]")]
public class LabelController : ControllerBase
{
	private readonly ILabelService labelService;
	private readonly ILogger<LabelController> logger;
	private readonly LabelFactory labelFactory;

	public LabelController(
		ILabelService labelService,
		ILogger<LabelController> logger,
		LabelFactory labelFactory)
	{
		this.labelService = labelService;
		this.logger = logger;
		this.labelFactory = labelFactory;
	}

	[HttpGet("All", Name = "GetAllLabels")]
	public async Task<IActionResult> GetAllLabels()
	{
		try
		{
			return Ok(await labelService.GetAll());
		}
		catch (Exception ex)
		{
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
	public async Task<ActionResult<Label>> CreateLabel([FromBody] InsertLabelDto dto)
	{
		return Created(
			"https://www.youtube.com/watch?v=LTyZKvIxrDg&t=3566s&ab_channel=Standuprs",
			await labelService.Insert(labelFactory.MapToDomain(dto)));
	}

	[HttpPut]
	public async Task<IActionResult> UpdateLabel([FromBody] UpdateLabelDto dto)
	{
		return (await labelService.Update(labelFactory.MapToDomain(dto)))
		.Map(label => (IActionResult)Ok(label))
		.GetValueOrDefault(() =>
		{
			return NotFound();
		});
	}

	[HttpDelete]
	public async Task<ActionResult<bool>> DeleteLabel([FromQuery] Guid id)
	{
		return Ok(await labelService.Delete(id));
	}
}
