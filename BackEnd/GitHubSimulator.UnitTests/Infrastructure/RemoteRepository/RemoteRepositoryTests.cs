using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Infrastructure.Configuration;
using GitHubSimulator.Infrastructure.RemoteRepository;
using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Core.WireProtocol.Messages;
using Moq;
using Moq.Protected;

namespace GitHubSimulator.UnitTests.Infrastructure.RemoteRepository
{
	public class RemoteRepositoryTests
	{
		private readonly Mock<IOptions<RemoteRepositorySettings>> _mockOptions;
		private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
		private readonly RemoteRepositoryService _service;

		public RemoteRepositoryTests()
		{
			_mockOptions = new Mock<IOptions<RemoteRepositorySettings>>();
			_mockOptions.Setup(x => x.Value).Returns(new RemoteRepositorySettings
			{
				BaseURL = "http://localhost:3000",
				AdminAccessToken = "testToken"
			});
			_mockHttpMessageHandler = new Mock<HttpMessageHandler>();

			var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
			_service = new RemoteRepositoryService(_mockOptions.Object, httpClient);
		}

		[Fact]
		public async Task GivenValidParameters_ForkRepo_ShouldNotThrowException()
		{
			// Arrange
			var username = "testUser";
			var owner = "testOwner";
			var repoName = "testRepo";
			var forkName = "testFork";

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
			Func<Task> act = async () => await _service.ForkRepo(username, owner, repoName, forkName);

			// Assert
			await act.Should().NotThrowAsync();
		}

		[Fact]
		public async Task GivenInvalidParameters_ForkRepo_ShouldThrowException()
		{
			// Arrange
			var username = "testUser";
			var owner = "testOwner";
			var repoName = "testRepo";
			var forkName = "testFork";

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
			Func<Task> act = async () => await _service.ForkRepo(username, owner, repoName, forkName);

			// Assert
			await act.Should().ThrowAsync<HttpRequestException>();
		}

		[Fact]
		public async Task GivenValidParameters_CreateUser_ShouldNotThrowException()
		{
			// Arrange
			GiteaUserDto user = new GiteaUserDto("test@email.com", "testUser", "testPassword", false, "Full Name");

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
			Func<Task> act = async () => await _service.CreateUser(user);

			// Assert
			await act.Should().NotThrowAsync();
		}

		[Fact]
		public async Task GivenInvalidParameters_CreateUser_ShouldThrowException()
		{
			GiteaUserDto user = new GiteaUserDto("test@email.com", "testUser", "testPassword", false, "Full Name");

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
			Func<Task> act = async () => await _service.CreateUser(user);

			// Assert
			await act.Should().ThrowAsync<HttpRequestException>();
		}

		[Fact]
		public async Task GivenValidRepository_CreateRepository_CreatesRepository()
		{
			// Arrange
			var username = "testUser";
			var repositoryDto = new CreateGiteaRepositoryDto("Test", "Test", false, "", "");

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
			Func<Task> act = async () => await _service.CreateRepository(username, repositoryDto);

			// Assert
			await act.Should().NotThrowAsync<HttpRequestException>();
		}

		[Fact]
		public async Task GivenInvalidRepository_CreateRepository_ShouldThrowException()
		{
			// Arrange
			var username = "testUser";
			var repositoryDto = new CreateGiteaRepositoryDto("Test", "Test", false, "", "");

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
			Func<Task> act = async () => await _service.CreateRepository(username, repositoryDto);

			// Assert
			await act.Should().ThrowAsync<HttpRequestException>();
		}

		[Fact]
		public async Task GivenValidParameters_AddCollaboratorToRepository_ShouldNotThrowException()
		{
			// Arrange
			string owner = "testOwner";
			string repo = "testRepo";
			string collaborator = "testCollaborator";

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
			Func<Task> act = async () => await _service.AddCollaboratorToRepository(owner, repo, collaborator);

			// Assert
			await act.Should().NotThrowAsync();
		}

		[Fact]
		public async Task GivenInvalidParameters_AddCollaboratorToRepository_ShouldThrowException()
		{
			string owner = "testOwner";
			string repo = "testRepo";
			string collaborator = "testCollaborator";

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
			Func<Task> act = async () => await _service.AddCollaboratorToRepository(owner, repo, collaborator);

			// Assert
			await act.Should().ThrowAsync<HttpRequestException>();
		}
	}
}

