using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using System.Net;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class ProfileService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly AuditLogger _auditLogger;

	public ProfileService(ApplicationDbContext dbContext, AuditLogger auditLogger)
	{
		_dbContext = dbContext;
		_auditLogger = auditLogger;
	}

	public async Task<GetPlayerProfile> GetProfileAsync(ClaimsPrincipal claimsPrincipal, int id)
	{
		string? userName = claimsPrincipal.GetName();
		UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
		if (user == null)
			throw new UnauthorizedAccessException();

		if (!user.PlayerId.HasValue)
			throw new InvalidProfileRequestException("User not linked to a player") { ShouldLog = false };

		if (user.PlayerId != id)
			throw new InvalidProfileRequestException("Not allowed to access another player's profile") { StatusCode = HttpStatusCode.Forbidden };

		PlayerEntity? player = await _dbContext.Players
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);
		if (player == null)
			return new();

		if (player.BanType != BanType.NotBanned)
			throw new InvalidProfileRequestException("Banned player");

		return new()
		{
			CountryCode = player.CountryCode,
			Dpi = player.Dpi,
			Fov = player.Fov,
			Gamma = player.Gamma,
			HasFlashHandEnabled = player.HasFlashHandEnabled,
			HideDonations = player.HideDonations,
			HidePastUsernames = player.HidePastUsernames,
			HideSettings = player.HideSettings,
			InGameSens = player.InGameSens,
			IsRightHanded = player.IsRightHanded,
			UsesHrtf = player.UsesHrtf,
			UsesInvertY = player.UsesInvertY,
			UsesLegacyAudio = player.UsesLegacyAudio,
			VerticalSync = player.VerticalSync,
		};
	}

	public async Task UpdateProfileAsync(ClaimsPrincipal claimsPrincipal, int id, EditPlayerProfile editPlayerProfile)
	{
		string? userName = claimsPrincipal.GetName();
		UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
		if (user == null)
			throw new UnauthorizedAccessException();

		if (!user.PlayerId.HasValue)
			throw new InvalidProfileRequestException("User not linked to a player") { ShouldLog = false };

		if (user.PlayerId != id)
			throw new InvalidProfileRequestException("Not allowed to access another player's profile") { StatusCode = HttpStatusCode.Forbidden };

		PlayerEntity? player = _dbContext.Players.FirstOrDefault(p => p.Id == id);
		if (player == null)
		{
			player = new() { Id = id };
			_dbContext.Players.Add(player);
		}
		else if (player.BanType != BanType.NotBanned)
		{
			throw new InvalidProfileRequestException("Banned player");
		}

		EditPlayerProfile oldDtoLog = new()
		{
			CountryCode = player.CountryCode,
			Dpi = player.Dpi,
			InGameSens = player.InGameSens,
			Fov = player.Fov,
			IsRightHanded = player.IsRightHanded,
			HasFlashHandEnabled = player.HasFlashHandEnabled,
			Gamma = player.Gamma,
			UsesLegacyAudio = player.UsesLegacyAudio,
			UsesHrtf = player.UsesHrtf,
			UsesInvertY = player.UsesInvertY,
			VerticalSync = player.VerticalSync,
			HideSettings = player.HideSettings,
			HideDonations = player.HideDonations,
			HidePastUsernames = player.HidePastUsernames,
		};

		player.CountryCode = editPlayerProfile.CountryCode;
		player.Dpi = editPlayerProfile.Dpi;
		player.Fov = editPlayerProfile.Fov;
		player.Gamma = editPlayerProfile.Gamma;
		player.HasFlashHandEnabled = editPlayerProfile.HasFlashHandEnabled;
		player.HideDonations = editPlayerProfile.HideDonations;
		player.HidePastUsernames = editPlayerProfile.HidePastUsernames;
		player.HideSettings = editPlayerProfile.HideSettings;
		player.InGameSens = editPlayerProfile.InGameSens;
		player.IsRightHanded = editPlayerProfile.IsRightHanded;
		player.UsesHrtf = editPlayerProfile.UsesHrtf;
		player.UsesInvertY = editPlayerProfile.UsesInvertY;
		player.UsesLegacyAudio = editPlayerProfile.UsesLegacyAudio;
		player.VerticalSync = editPlayerProfile.VerticalSync;

		await _dbContext.SaveChangesAsync();

		await _auditLogger.LogEdit(oldDtoLog.GetLog(), editPlayerProfile.GetLog(), claimsPrincipal, player.Id);
	}
}
