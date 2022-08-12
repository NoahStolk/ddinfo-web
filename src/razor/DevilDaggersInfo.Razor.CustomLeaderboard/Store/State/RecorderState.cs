using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Razor.CustomLeaderboard.Enums;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;

public record RecorderState(RecorderStateType State, long? Marker, string? UploadError, GetUploadSuccess? UploadSuccess, DateTime? LastSuccessfulUpload);
