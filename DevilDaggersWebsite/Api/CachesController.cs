//using DevilDaggersWebsite.Api.Attributes;
//using DevilDaggersWebsite.Caches;
//using DevilDaggersWebsite.Caches.LeaderboardHistory;
//using DevilDaggersWebsite.Caches.LeaderboardStatistics;
//using DevilDaggersWebsite.Caches.ModArchive;
//using DevilDaggersWebsite.Caches.SpawnsetData;
//using DevilDaggersWebsite.Caches.SpawnsetHash;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace DevilDaggersWebsite.Api
//{
//	[Route("api/caches")]
//	[ApiController]
//	public class CachesController : ControllerBase
//	{
//		private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;
//		private readonly LeaderboardHistoryCache _leaderboardHistoryCache;
//		private readonly ModArchiveCache _modArchiveCache;
//		private readonly SpawnsetDataCache _spawnsetDataCache;
//		private readonly SpawnsetHashCache _spawnsetHashCache;

//		public CachesController(
//			LeaderboardStatisticsCache leaderboardStatisticsCache,
//			LeaderboardHistoryCache leaderboardHistoryCache,
//			ModArchiveCache modArchiveCache,
//			SpawnsetDataCache spawnsetDataCache,
//			SpawnsetHashCache spawnsetHashCache)
//		{
//			_leaderboardStatisticsCache = leaderboardStatisticsCache;
//			_leaderboardHistoryCache = leaderboardHistoryCache;
//			_modArchiveCache = modArchiveCache;
//			_spawnsetDataCache = spawnsetDataCache;
//			_spawnsetHashCache = spawnsetHashCache;
//		}

//		[HttpPost("clear")]
//		[Authorize(Policies.AdminPolicy)]
//		[ProducesResponseType(StatusCodes.Status200OK)]
//		[EndpointConsumer(EndpointConsumers.Admin)]
//		public async Task<ActionResult> ClearCache(CacheType cacheType)
//		{
//			switch (cacheType)
//			{
//				case CacheType.LeaderboardStatistics: await _leaderboardStatisticsCache.Initiate(); break;
//				case CacheType.LeaderboardHistory: _leaderboardHistoryCache.Clear(); break;
//				case CacheType.ModArchive: _modArchiveCache.Clear(); break;
//				case CacheType.SpawnsetData: _spawnsetDataCache.Clear(); break;
//				case CacheType.SpawnsetHash: _spawnsetHashCache.Clear(); break;
//			}

//			return Ok();
//		}
//	}
//}
