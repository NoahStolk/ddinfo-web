using DevilDaggersInfo.Api.Admin.Mods;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Mods;

public class BinaryDataState : IStateObject<BinaryData>
{
	/// <summary>
	/// This name should not contain the type prefix or the mod name.
	/// </summary>
	[StringLength(64, MinimumLength = 1)]
	public string Name { get; set; } = null!;

	public byte[] Data { get; set; } = null!;

	public BinaryData ToModel() => new()
	{
		Name = Name,
		Data = Data,
	};
}
