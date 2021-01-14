using System;

namespace DevilDaggersWebsite.Dto
{
	public class TaskResult
	{
		public TaskResult(string typeName, DateTime lastTriggered, DateTime lastFinished, TimeSpan executionTime, string schedule)
		{
			TypeName = typeName;
			LastTriggered = lastTriggered;
			LastFinished = lastFinished;
			ExecutionTime = executionTime;
			Schedule = schedule;
		}

		public string TypeName { get; }
		public DateTime LastTriggered { get; }
		public DateTime LastFinished { get; }
		public TimeSpan ExecutionTime { get; }
		public string Schedule { get; }
	}
}
