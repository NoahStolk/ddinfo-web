using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Asset.Extensions;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Razor.AssetEditor.Data;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Razor.AssetEditor.Services;

public class BinaryState
{
	private readonly Dictionary<string, bool> _sorting = new();

	private Dictionary<AssetKey, VisualAsset> _visualChunks = new();

	public IReadOnlyDictionary<AssetKey, VisualAsset> VisualChunks => _visualChunks;

	public bool IsEmpty => _visualChunks.Count == 0;

	public bool IsSelectionEmpty => !_visualChunks.Any(kvp => kvp.Value.IsSelected);

	public ModBinary Binary { get; private set; } = new(ModBinaryType.Dd);

	public string BinaryName { get; set; } = "(Untitled)";

	public void SetBinary(ModBinary binary)
	{
		Binary = binary;
		RebuildChunkVisualization();
	}

	public void AddAsset(string assetName, AssetType assetType, byte[] fileContents)
	{
		Binary.AddAsset(assetName, assetType, fileContents);
		RebuildChunkVisualization();
	}

	private void RebuildChunkVisualization()
	{
		_visualChunks.Clear();

		foreach (ModBinaryChunk chunk in Binary.Chunks)
			_visualChunks.Add(new(chunk.AssetType, chunk.Name), new(chunk.Size, AssetContainer.GetIsProhibited(chunk.AssetType, chunk.Name)));
	}

	public void ResetSelection(IEnumerable<AssetKey> keys)
	{
		foreach (KeyValuePair<AssetKey, VisualAsset> kvp in _visualChunks)
			kvp.Value.IsSelected = keys.Contains(kvp.Key);
	}

	public void InvertSelection()
	{
		foreach (KeyValuePair<AssetKey, VisualAsset> kvp in _visualChunks)
			kvp.Value.IsSelected = !kvp.Value.IsSelected;
	}

	public void ToggleSelection(VisualAsset asset)
	{
		asset.IsSelected = !asset.IsSelected;
	}

	public void DeleteChunks()
	{
		foreach (KeyValuePair<AssetKey, VisualAsset> kvp in _visualChunks.Where(kvp => kvp.Value.IsSelected))
		{
			_visualChunks.Remove(kvp.Key);
			Binary.RemoveAsset(kvp.Key);
		}

		RebuildChunkVisualization();
	}

	public void ExtractChunks(string directory)
	{
		foreach (AssetKey assetKey in _visualChunks.Where(kvp => kvp.Value.IsSelected).Select(kvp => kvp.Key))
		{
			byte[] extractedBuffer = Binary.ExtractAsset(assetKey);

			if (assetKey.AssetType == AssetType.Shader)
			{
				using MemoryStream ms = new(extractedBuffer);
				using BinaryReader br = new(ms);

				int nameLength = br.ReadInt32();
				int vertexSize = br.ReadInt32();
				int fragmentSize = br.ReadInt32();
				_ = br.ReadBytes(nameLength); // Name
				byte[] vertexBuffer = br.ReadBytes(vertexSize);
				byte[] fragmentBuffer = br.ReadBytes(fragmentSize);

				File.WriteAllBytes(Path.Combine(directory, $"{assetKey.AssetName}.vert"), vertexBuffer);
				File.WriteAllBytes(Path.Combine(directory, $"{assetKey.AssetName}.frag"), fragmentBuffer);
			}
			else
			{
				string fileName = assetKey.AssetName + assetKey.AssetType.GetFileExtension();
				File.WriteAllBytes(Path.Combine(directory, fileName), extractedBuffer);
			}
		}
	}

	public void EnableChunks()
	{
		foreach (AssetKey assetKey in _visualChunks.Select(kvp => kvp.Key))
			Binary.EnableAsset(assetKey);
	}

	public void DisableChunks()
	{
		foreach (AssetKey assetKey in _visualChunks.Select(kvp => kvp.Key))
			Binary.DisableAsset(assetKey);
	}

	public void Sort<TKey>(Func<KeyValuePair<AssetKey, VisualAsset>, TKey> sorting, [CallerArgumentExpression("sorting")] string sortingExpression = "")
	{
		bool sortDirection = false;
		if (_sorting.ContainsKey(sortingExpression))
			sortDirection = _sorting[sortingExpression];
		else
			_sorting.Add(sortingExpression, false);

		_visualChunks = _visualChunks.OrderBy(sorting, sortDirection).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		_sorting[sortingExpression] = !sortDirection;
	}
}
