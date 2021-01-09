using System;

namespace DevilDaggersWebsite.LeaderboardHistory
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class CompletionPropertyAttribute : Attribute
	{
	}
}