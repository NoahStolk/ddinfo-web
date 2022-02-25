using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using System.Net;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class ProfileService
{
	private readonly ApplicationDbContext _dbContext;

	public ProfileService(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task UpdateProfile(ClaimsPrincipal claimsPrincipal, int id, EditPlayerProfile editPlayerProfile)
	{
		string? userName = claimsPrincipal.GetName();
		UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
		if (user == null)
			throw new HttpRequestException("Unauthorized", null, HttpStatusCode.Unauthorized);

		if (user.PlayerId != id)
			throw new HttpRequestException("Not allowed to edit another player's profile", null, HttpStatusCode.Forbidden);

		PlayerEntity? player = _dbContext.Players
			.AsNoTracking()
			.FirstOrDefault(p => p.Id == id);
		if (player == null)
			throw new HttpRequestException("Player not found", null, HttpStatusCode.NotFound);

		if (player.BanType != BanType.NotBanned)
			throw new HttpRequestException("Banned player", null, HttpStatusCode.BadRequest);

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
	}
}
