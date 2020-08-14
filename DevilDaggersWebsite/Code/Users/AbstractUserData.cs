using Newtonsoft.Json;
using System;

namespace DevilDaggersWebsite.Code.Users
{
	[Obsolete("Moved to database.")]
	public abstract class AbstractUserData
	{
		[JsonIgnore]
		public abstract string FileName { get; }
	}
}