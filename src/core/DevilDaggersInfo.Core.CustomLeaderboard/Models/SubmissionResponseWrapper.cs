using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

namespace DevilDaggersInfo.Core.CustomLeaderboard.Models;

public class SubmissionResponseWrapper
{
	private readonly GetUploadSuccess? _response;

	public SubmissionResponseWrapper(GetUploadSuccess response)
	{
		_response = response;
	}

	public SubmissionResponseWrapper(string errorMessage)
	{
		ErrorMessage = errorMessage;
	}

	public string? ErrorMessage { get; }

	public bool HasError => ErrorMessage != null;

	/// <summary>
	/// Returns the fetched response, or throws an <see cref="InvalidOperationException"/> when the response is <see langword="null"/>.
	/// Make sure to check <see cref="HasError"/> before calling this method.
	/// </summary>
	public GetUploadSuccess Response
	{
		get
		{
			if (_response == null)
				throw new InvalidOperationException($"Cannot get response because it is null. Make sure to check for an error before calling this method. This instance holds the following error: {ErrorMessage}");

			return _response;
		}
	}
}
