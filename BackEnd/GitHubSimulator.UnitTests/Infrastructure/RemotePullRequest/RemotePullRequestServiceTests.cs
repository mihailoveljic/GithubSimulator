using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using GitHubSimulator.Infrastructure.Configuration;
using GitHubSimulator.Infrastructure.RemotePullRequest;
using GitHubSimulator.Infrastructure.RemotePullRequest.Dtos;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace GitHubSimulator.UnitTests.Infrastructure.RemotePullRequest
{
	public class RemotePullRequestServiceTests
	{
		private readonly Mock<IOptions<RemoteRepositorySettings>> _mockOptions;
		private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
		private readonly RemotePullRequestService _service;

		public RemotePullRequestServiceTests()
		{
			_mockOptions = new Mock<IOptions<RemoteRepositorySettings>>();
			_mockOptions.Setup(x => x.Value).Returns(new RemoteRepositorySettings
			{
				BaseURL = "http://localhost:3000",
				AdminAccessToken = "testToken"
			});
			_mockHttpMessageHandler = new Mock<HttpMessageHandler>();

			var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
			_service = new RemotePullRequestService(_mockOptions.Object, httpClient);
		}

		[Fact]
		public async Task GivenValidParameters_CreatePullRequest_ShouldNotThrowException()
		{
			// Arrange
			var username = "testUser";
			var repo = "testRepo";
			var pullRequestDto = new CreateGiteaPullRequest("", "", "", "", "");
			GiteaPullRequestDto pullRequest = new GiteaPullRequestDto(0, 0, "", "", "", true, true, "", "", "", "", "", "", new BranchDTO("", 0, ""), new BranchDTO("", 0, ""));

			var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
			responseMessage.Content = JsonContent.Create(pullRequest);
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(responseMessage);

			// Act
			Func<Task> act = async () => await _service.CreatePullRequest(username, repo, pullRequestDto);

			// Assert
			await act.Should().NotThrowAsync();
		}

		[Fact]
		public async Task GivenValidParameters_CreatePullRequest_ShouldThrowException()
		{
			// Arrange
			var username = "testUser";
			var repo = "testRepo";
			var pullRequestDto = new CreateGiteaPullRequest("", "", "", "", "");
			GiteaPullRequestDto pullRequest = new GiteaPullRequestDto(0, 0, "", "", "", true, true, "", "", "", "", "", "", new BranchDTO("", 0, ""), new BranchDTO("", 0, ""));

			var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
			responseMessage.Content = JsonContent.Create(pullRequest);
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(responseMessage);

			// Act
			Func<Task> act = async () => await _service.CreatePullRequest(username, repo, pullRequestDto);

			// Assert
			await act.Should().ThrowAsync<HttpRequestException>();
		}

		[Fact]
		public async Task GivenValidParameters_GetPullRequest_ShouldNotThrowException()
		{
			// Arrange
			var username = "testUser";
			var repo = "testRepo";
			var index = "1";
			GiteaPullRequestDto pullRequest = new GiteaPullRequestDto(0, 0, "", "", "", true, true, "", "", "", "", "", "", new BranchDTO("", 0, ""), new BranchDTO("", 0, ""));

			var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
			responseMessage.Content = JsonContent.Create(pullRequest);
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(responseMessage);

			// Act
			Func<Task> act = async () => await _service.GetPullRequest(username, repo, index);

			// Assert
			await act.Should().NotThrowAsync();
		}

		[Fact]
		public async Task GivenValidParameters_GetPullRequest_ShouldThrowException()
		{
			// Arrange
			var username = "testUser";
			var repo = "testRepo";
			var index = "1";
			GiteaPullRequestDto pullRequest = new GiteaPullRequestDto(0, 0, "", "", "", true, true, "", "", "", "", "", "", new BranchDTO("", 0, ""), new BranchDTO("", 0, ""));

			var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
			responseMessage.Content = JsonContent.Create(pullRequest);
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(responseMessage);

			// Act
			Func<Task> act = async () => await _service.GetPullRequest(username, repo, index);

			// Assert
			await act.Should().ThrowAsync<HttpRequestException>();
		}

		[Fact]
		public async Task GivenValidParameters_CommitDiff_ShouldNotThrowException()
		{
			// Arrange
			var username = "testUser";
			var repo = "testRepo";
			var sha = "testSha";
			var diff = "testDiff";

			var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
			responseMessage.Content = new StringContent(diff);
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(responseMessage);

			// Act
			Func<Task> act = async () => await _service.CommitDiff(username, repo, sha);

			// Assert
			await act.Should().NotThrowAsync();
		}

		[Fact]
		public async Task GivenInvalidParameters_CommitDiff_ShouldThrowException()
		{
			// Arrange
			var username = "testUser";
			var repo = "testRepo";
			var sha = "testSha";
			var diff = "testDiff";

			var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
			responseMessage.Content = new StringContent(diff);
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(responseMessage);

			// Act
			Func<Task> act = async () => await _service.CommitDiff(username, repo, sha);

			// Assert
			await act.Should().ThrowAsync<HttpRequestException>();
		}

		[Fact]
		public async Task GivenValidParameters_MergePullRequest_ShouldNotThrowException()
		{
			// Arrange
			var username = "testUser";
			var repo = "testRepo";
			var index = "1";
			var mergePullRequestDto = new MergeGiteaPullRequest("", "", "", "", false, false, "", false);

			var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(responseMessage);

			// Act
			Func<Task> act = async () => await _service.MergePullRequest(username, repo, index, mergePullRequestDto);

			// Assert
			await act.Should().NotThrowAsync();
		}

		[Fact]
		public async Task GivenInvalidParameters_MergePullRequest_ShouldThrowException()
		{
			// Arrange
			var username = "testUser";
			var repo = "testRepo";
			var index = "1";
			var mergePullRequestDto = new MergeGiteaPullRequest("", "", "", "", false, false, "", false);

			var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
			_mockHttpMessageHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(responseMessage);

			// Act
			Func<Task> act = async () => await _service.MergePullRequest(username, repo, index, mergePullRequestDto);

			// Assert
			await act.Should().ThrowAsync<HttpRequestException>();
		}
	}
}
