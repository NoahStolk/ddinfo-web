using Newtonsoft.Json;

namespace DevilDaggersWebsite.Code.Users
{
	public abstract class AbstractUserData
	{
		[JsonIgnore]
		public abstract string FileName { get; }
	}
}