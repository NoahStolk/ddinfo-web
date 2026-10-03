using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.Server;

internal sealed class ExceptionMiddlewareTests
{
	[Test]
	public async Task DdLeaderboardException_HidesInnerExceptionMessages()
	{
		DdLeaderboardException exception = new("The leaderboard servers could not be reached.", new TaskCanceledException("A task was canceled."));

		(int statusCode, string? title) = await InvokeAsync(exception);

		await Assert.That(statusCode).IsEqualTo(StatusCodes.Status503ServiceUnavailable);
		await Assert.That(title).IsEqualTo("The leaderboard servers could not be reached.");
	}

	[Test]
	public async Task StatusCodeException_IncludesInnerExceptionMessages()
	{
		InvalidModArchiveException exception = new("Processing the mod archive failed.", new InvalidOperationException("Invalid binary."));

		(int statusCode, string? title) = await InvokeAsync(exception);

		await Assert.That(statusCode).IsEqualTo(StatusCodes.Status400BadRequest);
		await Assert.That(title).IsEqualTo($"Processing the mod archive failed.{Environment.NewLine}Invalid binary.");
	}

	private static async Task<(int StatusCode, string? Title)> InvokeAsync(StatusCodeException exception)
	{
		ExceptionMiddleware middleware = new(_ => throw exception, NullLogger<ExceptionMiddleware>.Instance);
		DefaultHttpContext context = new();
		using MemoryStream body = new();
		context.Response.Body = body;

		await middleware.InvokeAsync(context);

		body.Position = 0;
		using JsonDocument document = await JsonDocument.ParseAsync(body);
		return (context.Response.StatusCode, document.RootElement.GetProperty("title").GetString());
	}
}
