using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Asset.Extensions;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using System;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Services;

public class BinaryState
{
	private readonly Dictionary<string, bool> _sorting = new();

	private readonly Dictionary<ModBinaryChunk, bool> _prohibited = new();
	private readonly List<ModBinaryChunk> _selectedChunks = new();
	private List<ModBinaryChunk> _visualChunks = new();

	public IReadOnlyDictionary<ModBinaryChunk, bool> Prohibited => _prohibited;
	public IReadOnlyList<ModBinaryChunk> SelectedChunks => _selectedChunks;
	public IReadOnlyList<ModBinaryChunk> VisualChunks => _visualChunks;

	public bool IsEmpty => Binary.Chunks.Count == 0;

	public bool IsSelectionEmpty => _selectedChunks.Count == 0;

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
		_prohibited.Clear();
		_selectedChunks.Clear();
		_visualChunks.Clear();

		_visualChunks = Binary.Chunks;

		foreach (ModBinaryChunk chunk in _visualChunks)
			_prohibited[chunk] = AssetContainer.GetIsProhibited(chunk.AssetType, chunk.Name);
	}

	public void ResetSelection(IEnumerable<ModBinaryChunk> chunks)
	{
		_selectedChunks.Clear();
		_selectedChunks.AddRange(chunks);
	}

	public void InvertSelection()
	{
		List<ModBinaryChunk> chunksToDeselect = new();
		chunksToDeselect.AddRange(_selectedChunks);

		_selectedChunks.Clear();
		_selectedChunks.AddRange(_visualChunks.Where(c => !chunksToDeselect.Any(ctd => ctd.Name == c.Name && ctd.AssetType == c.AssetType)));
	}

	public void ToggleSelection(ModBinaryChunk chunk)
	{
		if (_selectedChunks.Contains(chunk))
			_selectedChunks.Remove(chunk);
		else
			_selectedChunks.Add(chunk);
	}

	public void DeleteChunks()
	{
		for (int i = _selectedChunks.Count - 1; i >= 0; i--)
		{
			ModBinaryChunk chunkToDelete = _selectedChunks[i];
			Binary.RemoveAsset(chunkToDelete.Name, chunkToDelete.AssetType);
			_selectedChunks.Remove(chunkToDelete);
		}

		RebuildChunkVisualization();
	}

	public void ExtractChunks(string directory)
	{
		foreach (ModBinaryChunk chunk in _selectedChunks)
		{
			byte[] extractedBuffer = Binary.ExtractAsset(chunk.Name, chunk.AssetType);

			if (chunk.AssetType == AssetType.Shader)
			{
				using MemoryStream ms = new(extractedBuffer);
				using BinaryReader br = new(ms);

				int nameLength = br.ReadInt32();
				int vertexSize = br.ReadInt32();
				int fragmentSize = br.ReadInt32();
				_ = br.ReadBytes(nameLength); // Name
				byte[] vertexBuffer = br.ReadBytes(vertexSize);
				byte[] fragmentBuffer = br.ReadBytes(fragmentSize);

				File.WriteAllBytes(Path.Combine(directory, $"{chunk.Name}.vert"), vertexBuffer);
				File.WriteAllBytes(Path.Combine(directory, $"{chunk.Name}.frag"), fragmentBuffer);
			}
			else
			{
				string fileName = chunk.Name + chunk.AssetType.GetFileExtension();
				File.WriteAllBytes(Path.Combine(directory, fileName), extractedBuffer);
			}
		}
	}

	public void EnableChunks()
	{
		foreach (ModBinaryChunk chunk in _selectedChunks)
			Binary.EnableAsset(chunk.Name, chunk.AssetType);
	}

	public void DisableChunks()
	{
		foreach (ModBinaryChunk chunk in _selectedChunks)
			Binary.DisableAsset(chunk.Name, chunk.AssetType);
	}

	public void Sort<TKey>(Func<ModBinaryChunk, TKey> sorting, [CallerArgumentExpression("sorting")] string sortingExpression = "")
	{
		bool sortDirection = false;
		if (_sorting.ContainsKey(sortingExpression))
			sortDirection = _sorting[sortingExpression];
		else
			_sorting.Add(sortingExpression, false);

		_visualChunks = _visualChunks.OrderBy(sorting, sortDirection).ToList();

		_sorting[sortingExpression] = !sortDirection;
	}
}
