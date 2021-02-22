using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Tasks
{
	public static class TaskInstanceKeeper
	{
		public static Dictionary<Type, AbstractTask> Instances { get; } = new();
	}
}
