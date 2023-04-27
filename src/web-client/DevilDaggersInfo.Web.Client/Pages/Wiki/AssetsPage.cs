using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Asset;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.Client.Pages.Wiki;

public partial class AssetsPage
{
	private IReadOnlyList<AudioAssetInfo> _audioAudio = AudioAudio2.All;
	private IReadOnlyList<MeshAssetInfo> _ddMeshes = DdMeshes2.All;
	private IReadOnlyList<ObjectBindingAssetInfo> _ddObjectBindings = DdObjectBindings2.All;
	private IReadOnlyList<ShaderAssetInfo> _coreShaders = CoreShaders2.All;
	private IReadOnlyList<ShaderAssetInfo> _ddShaders = DdShaders2.All;
	private IReadOnlyList<TextureAssetInfo> _ddTextures = DdTextures2.All;

	private readonly Dictionary<string, bool> _audioAudioSortings = new();
	private readonly Dictionary<string, bool> _ddMeshesSortings = new();
	private readonly Dictionary<string, bool> _ddObjectBindingsSortings = new();
	private readonly Dictionary<string, bool> _coreShadersSortings = new();
	private readonly Dictionary<string, bool> _ddShadersSortings = new();
	private readonly Dictionary<string, bool> _ddTexturesSortings = new();

	private void Sort<TSource, TKey>(ref List<TSource> source, Dictionary<string, bool> sortings, Func<TSource, TKey> sorting, [CallerArgumentExpression("sorting")] string sortingExpression = "")
	{
		bool sortDirection = false;
		if (sortings.ContainsKey(sortingExpression))
			sortDirection = sortings[sortingExpression];
		else
			sortings.Add(sortingExpression, false);

		source = source.OrderBy(sorting, sortDirection).ToList();

		sortings[sortingExpression] = !sortDirection;
	}
}
