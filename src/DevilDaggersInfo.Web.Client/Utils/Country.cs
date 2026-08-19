namespace DevilDaggersInfo.Web.Client.Utils;

/// <summary>
/// A country code that is known to have both a display name and a flag image.
/// </summary>
internal readonly record struct Country(string Code, string Name, string FlagPath);
