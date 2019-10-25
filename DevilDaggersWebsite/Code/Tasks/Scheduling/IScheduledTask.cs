using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Tasks.Scheduling
{
	public interface IScheduledTask
	{
		string Schedule { get; }
		Task ExecuteAsync(CancellationToken cancellationToken);
	}
}