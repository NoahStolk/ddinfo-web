using System;

namespace DevilDaggersWebsite.Code.DataTransferObjects
{
	public class VersionResult
	{
		public VersionResult(bool? isUpToDate, Tool tool, Exception? exception = null)
		{
			IsUpToDate = isUpToDate;
			Tool = tool;
			Exception = exception;
		}

		/// <summary>
		/// True if the application is up to date, false if not, null if not known.
		/// </summary>
		public bool? IsUpToDate { get; set; }

		/// <summary>
		/// The <see cref="DataTransferObjects.Tool"/> retrieved from the website, or null if failed.
		/// </summary>
		public Tool Tool { get; set; }

		/// <summary>
		/// The Exception that occurred if attempting to retrieve the version number failed.
		/// </summary>
		public Exception? Exception { get; set; }
	}
}