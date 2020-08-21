using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Core.Dto
{
	public class WebStatsResult
	{
		public WebStatsResult(DateTime buildDateTime, List<TaskResult> taskResults)
		{
			WebsiteBuildDateTime = buildDateTime;
			TaskResults = taskResults;
		}

		public DateTime WebsiteBuildDateTime { get; }
		public List<TaskResult> TaskResults { get; }
	}
}