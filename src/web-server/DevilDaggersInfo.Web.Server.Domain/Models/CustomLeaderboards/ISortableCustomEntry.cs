namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public interface ISortableCustomEntry
{
	int Time { get; }

	DateTime SubmitDate { get; }
}
