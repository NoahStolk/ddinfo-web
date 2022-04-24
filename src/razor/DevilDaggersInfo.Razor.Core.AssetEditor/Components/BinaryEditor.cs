using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Asset.Extensions;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Components;

public partial class BinaryEditor
{
	private readonly Dictionary<string, bool> _sorting = new();

	private readonly List<ModBinaryChunk> _selectedChunks = new();
	private readonly Dictionary<ModBinaryChunk, bool> _prohibited = new();
	private List<ModBinaryChunk> _chunks = new();

	private ModBinary _binary = null!;

	private bool _renderAddAsset;

	public bool AddingNewAsset
	{
		get => _renderAddAsset;
		set
		{
			_renderAddAsset = value;
			RebuildChunkVisualization();
			StateHasChanged();
		}
	}

	public bool IsEmpty => _chunks.Count == 0;

	public bool IsSelectionEmpty => _selectedChunks.Count == 0;

	[Parameter]
	[EditorRequired]
	public string? BinaryName { get; set; }

	[Parameter]
	[EditorRequired]
	public ModBinary Binary
	{
		get => _binary;
		set
		{
			_binary = value;
			RebuildChunkVisualization();
		}
	}

	[Inject]
	public IErrorReporter ErrorReporter { get; set; } = null!;

	[Inject]
	public IFileSystemService FileSystemService { get; set; } = null!;

	private void RebuildChunkVisualization()
	{
		_selectedChunks.Clear();
		_prohibited.Clear();
		_chunks = _binary.Chunks;

		foreach (ModBinaryChunk chunk in _chunks)
			_prohibited[chunk] = AssetContainer.GetIsProhibited(chunk.AssetType, chunk.Name);
	}

	private void ResetSelection(IEnumerable<ModBinaryChunk> chunks)
	{
		_selectedChunks.Clear();
		_selectedChunks.AddRange(chunks);
	}

	private void InvertSelection()
	{
		List<ModBinaryChunk> chunksToDeselect = new();
		chunksToDeselect.AddRange(_selectedChunks);

		_selectedChunks.Clear();
		_selectedChunks.AddRange(_chunks.Where(c => !chunksToDeselect.Any(ctd => ctd.Name == c.Name && ctd.AssetType == c.AssetType)));
	}

	private void ToggleSelection(ModBinaryChunk chunk)
	{
		if (_selectedChunks.Contains(chunk))
			_selectedChunks.Remove(chunk);
		else
			_selectedChunks.Add(chunk);
	}

	private void DeleteChunks()
	{
		for (int i = _selectedChunks.Count - 1; i >= 0; i--)
		{
			ModBinaryChunk chunkToDelete = _selectedChunks[i];
			Binary.RemoveAsset(chunkToDelete.Name, chunkToDelete.AssetType);
			_selectedChunks.Remove(chunkToDelete);
		}
	}

	private void ExtractChunks()
	{
		string? directory = FileSystemService.SelectDirectory();
		if (directory == null)
			return;

		foreach (ModBinaryChunk chunk in _selectedChunks)
		{
			string fileName = chunk.Name + chunk.AssetType.GetFileExtension();
			byte[] extractedBuffer;
			try
			{
				extractedBuffer = Binary.ExtractAsset(chunk.Name, chunk.AssetType);
			}
			catch (Exception ex)
			{
				ErrorReporter.ReportError(ex);
				continue;
			}

			File.WriteAllBytes(Path.Combine(directory, fileName), extractedBuffer);
		}
	}

	private void EnableChunks()
	{
		foreach (ModBinaryChunk chunk in _selectedChunks)
			chunk.Enable();
	}

	private void DisableChunks()
	{
		foreach (ModBinaryChunk chunk in _selectedChunks)
			chunk.Disable();
	}

	public static string GetBgColor(AssetType assetType) => $"bg-{GetColor(assetType)}";

	public static string GetTextColor(AssetType assetType) => $"text-{GetColor(assetType)}";

	private static string GetColor(AssetType assetType) => assetType switch
	{
		AssetType.Audio => "audio",
		AssetType.ObjectBinding => "object-binding",
		AssetType.Shader => "shader",
		AssetType.Texture => "texture",
		AssetType.Mesh => "mesh",
		_ => "black",
	};

	private void Sort<TKey>(Func<ModBinaryChunk, TKey> sorting, [CallerArgumentExpression("sorting")] string sortingExpression = "")
	{
		bool sortDirection = false;
		if (_sorting.ContainsKey(sortingExpression))
			sortDirection = _sorting[sortingExpression];
		else
			_sorting.Add(sortingExpression, false);

		_chunks = _chunks.OrderBy(sorting, sortDirection).ToList();

		_sorting[sortingExpression] = !sortDirection;
	}
}
