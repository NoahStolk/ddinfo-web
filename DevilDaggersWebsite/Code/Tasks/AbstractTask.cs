using DevilDaggersWebsite.Code.Tasks.Scheduling;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Tasks
{
	public abstract class AbstractTask : IScheduledTask
	{
		public DateTime LastTriggered { get; private set; }
		public DateTime LastFinished { get; private set; }
		public TimeSpan ExecutionTime { get; private set; }

		public abstract string Schedule { get; }

		protected AbstractTask()
		{
			TaskInstanceKeeper.Instances[GetType()] = this;
		}

		protected abstract Task Execute();

		public async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			LastTriggered = DateTime.Now;

			await Execute();

			ExecutionTime = stopwatch.Elapsed;

			LastFinished = DateTime.Now;
		}
	}
}