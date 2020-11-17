using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Core.Tasks
{
	public static class TaskInstanceKeeper
	{
		public static Dictionary<Type, AbstractTask> Instances { get; } = new Dictionary<Type, AbstractTask>();
	}
}