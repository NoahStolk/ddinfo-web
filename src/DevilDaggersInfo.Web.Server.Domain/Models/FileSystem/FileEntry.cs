namespace DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;

/// <summary>
/// A file listed by <see cref="Services.Inversion.IFileSystem.ListAsync"/>.
/// </summary>
/// <param name="Name">The name of the file relative to its <see cref="DataSubDirectory"/>, using <c>/</c> as separator for nested files.</param>
/// <param name="Size">The size of the file in bytes.</param>
public sealed record FileEntry(string Name, long Size);
