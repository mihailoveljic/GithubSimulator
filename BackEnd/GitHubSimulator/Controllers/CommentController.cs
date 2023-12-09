using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using GitHubSimulator.Core.BuildingBlocks.Exceptions;
using GitHubSimulator.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Comments;
using GitHubSimulator.Factories;

namespace GitHubSimulator.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    public CommentController(ICommentService commentService, CommentFactory commentFactory)
    {
        _commentService = commentService;
        _commentFactory = commentFactory;
    }

    private readonly ICommentService _commentService;
    private readonly CommentFactory _commentFactory;

    [HttpGet(Name = "GetAllComments")]
    public async Task<IActionResult> GetAllComments()
    {
        try
        {
            return Ok(await _commentService.GetAll());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
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
            var result = await _commentService.Insert(_commentFactory.MapToDomain(dto));
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
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentDto dto)
    {
        try
        {
            var response = await _commentService.Update(_commentFactory.MapToDomain(dto));
            if (response.Equals(Maybe<Comment>.None))
            {
                return NotFound("A comment with the provided ID not found");
            }

            return Ok(response.Value);
        }
        catch (FluentValidation.ValidationException ve)
        {
            return BadRequest(ve.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        var response = await _commentService.Delete(id);
        if (response)
        {
            return Ok("Comment deleted successfully");
        }

        return NotFound("A comment with provided ID not found");
    }
}