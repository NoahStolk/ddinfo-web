using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Core.Tasks
{
	public abstract class HostedService : IHostedService
	{
		// Example untested base class code kindly provided by David Fowler: https://gist.github.com/davidfowl/a7dd5064d9dcf35b6eae1a7953d615e3

		private Task executingTask;
		private CancellationTokenSource cts;

		public Task StartAsync(CancellationToken cancellationToken)
		{
			// Create a linked token so we can trigger cancellation outside of this token's cancellation.
			cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

			// Store the task we're executing.
			executingTask = ExecuteAsync(cts.Token);

			// If the task is completed then return it, otherwise it's running.
			return executingTask.IsCompleted ? executingTask : Task.CompletedTask;
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			// Stop was called without start.
			if (executingTask == null)
				return;

			// Signal cancellation to the executing method.
			cts.Cancel();

			// Wait until the task completes or the stop token triggers.
			await Task.WhenAny(executingTask, Task.Delay(-1, cancellationToken));

			// Throw if cancellation triggered.
			cancellationToken.ThrowIfCancellationRequested();
		}

		// Derived classes should override this and execute a long running method until cancellation is requested.
		protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
	}
}