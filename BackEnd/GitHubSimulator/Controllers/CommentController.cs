using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using GitHubSimulator.Core.BuildingBlocks.Exceptions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Comments;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CommentController : ControllerBase
{
	public CommentController(
        ICommentService commentService, 
        CommentFactory commentFactory, 
        ICacheService cacheService, 
        ILogger<CommentController> logger)
	{
		_commentService = commentService;
		_commentFactory = commentFactory;
		_cacheService = cacheService;
        _logger = logger;
	}

	private readonly ICommentService _commentService;
	private readonly CommentFactory _commentFactory;
	private readonly ICacheService _cacheService;
	private readonly ILogger<CommentController> _logger;

    [HttpGet(Name = "GetAllComments")]
    public async Task<IActionResult> GetAllComments()
    {
        try
        {
            var cachedComments = await _cacheService.GetAllCommentsAsync();

            if (cachedComments != null && cachedComments.Any())
            {
                _logger.LogInformation("Serving comments from cache");
                return Ok(cachedComments);
            }

            var comments = await _commentService.GetAll();
            if (comments.Any())
            {
                foreach (var comment in comments)
                {
                    await _cacheService.SetCommentData(comment, DateTimeOffset.UtcNow.AddHours(2));
                }
            }
            _logger.LogInformation("Serving comments from database");
            return Ok(comments);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching all comments");
            return BadRequest(e.Message);
        }
    }




    [HttpGet("{id:guid}", Name = "GetCommentById")]
	[SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
	public async Task<IActionResult> GetCommentById(Guid id)
	{
		var response = await _commentService.GetById(id);
		if (response is null)
		{
			return NotFound("A comment with the provided ID not found");
		}

		return Ok(response);
	}

    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] InsertCommentDto dto)
    {
        try
        {
            var comment = _commentFactory.MapToDomain(dto);
            var result = await _commentService.Insert(comment);

            // Invalidate cache after successful creation
            await _cacheService.RemoveAllCommentDataAsync();

            _logger.LogInformation("Comment successfully created");
            return Created("Comment successfully created", result);
        }
        catch (InvalidTaskForCommentException te)
        {
            return BadRequest(te.Message);
        }
        catch (FluentValidation.ValidationException ve)
        {
            return BadRequest("Fluent validation error: " + ve.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal Server Error during comment creation");
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }


    [HttpPut]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentDto dto)
    {
        try
        {
            var commentUpdate = _commentFactory.MapToDomain(dto);
            var response = await _commentService.Update(commentUpdate);

            if (response.Equals(Maybe<Comment>.None))
            {
                return NotFound("A comment with the provided ID not found");
            }

            // Invalidate cache after successful update
            await _cacheService.RemoveAllCommentDataAsync();

            _logger.LogInformation("Comment successfully updated");
            return Ok(response.Value);
        }
        catch (FluentValidation.ValidationException ve)
        {
            return BadRequest(ve.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating comment");
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        try
        {
            var response = await _commentService.Delete(id);
            if (response)
            {
                await _cacheService.RemoveAllCommentDataAsync();
                _logger.LogInformation("Comment deleted successfully");
                return Ok("Comment deleted successfully");
            }

            return NotFound("A comment with the provided ID not found");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting comment");
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }

}