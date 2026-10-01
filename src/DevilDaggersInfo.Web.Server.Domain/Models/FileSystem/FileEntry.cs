namespace DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;

/// <param name="Name">The name of the file relative to its <see cref="DataSubDirectory"/>, using <c>/</c> as separator for nested files.</param>
/// <param name="Size">The size of the file in bytes.</param>
public sealed record FileEntry(string Name, long Size);
