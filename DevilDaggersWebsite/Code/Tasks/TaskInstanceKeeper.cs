using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.Tasks
{
	public static class TaskInstanceKeeper
	{
		public static Dictionary<Type, AbstractTask> Instances { get; private set; } = new Dictionary<Type, AbstractTask>();
	}
}