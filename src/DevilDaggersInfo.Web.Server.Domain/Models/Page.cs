namespace DevilDaggersInfo.Web.Server.Domain.Models;

public sealed record Page<T>(List<T> Results, int TotalResults);
