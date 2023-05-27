namespace DevilDaggersInfo.Web.Server.Domain.Models;

public record Page<T>(List<T> Results, int TotalResults);
