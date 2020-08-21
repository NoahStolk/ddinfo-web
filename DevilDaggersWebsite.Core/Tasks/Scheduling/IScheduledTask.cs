using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Core.Tasks.Scheduling
{
	public interface IScheduledTask
	{
		string Schedule { get; }

		Task ExecuteAsync(CancellationToken cancellationToken);
	}
}