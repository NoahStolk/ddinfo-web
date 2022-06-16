namespace DevilDaggersInfo.Razor.CustomLeaderboard.Models;

public class ResponseWrapper<T>
{
	private readonly T? _response;

	public ResponseWrapper(T response)
	{
		_response = response;
	}

	public ResponseWrapper(string errorMessage)
	{
		ErrorMessage = errorMessage;
	}

	public string? ErrorMessage { get; }

	public bool HasError => ErrorMessage != null;

	/// <summary>
	/// Returns the fetched response, or throws an <see cref="InvalidOperationException"/> when the response is <see langword="null"/>.
	/// Make sure to check <see cref="HasError"/> before calling this method.
	/// </summary>
	public T Response
	{
		get
		{
			if (_response == null)
				throw new InvalidOperationException($"Cannot get response because it is null. Make sure to check for an error before calling this method. This instance holds the following error: {ErrorMessage}");

			return _response;
		}
	}
}
