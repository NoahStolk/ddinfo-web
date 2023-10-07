using DevilDaggersInfo.Core.Common.Extensions;
using DevilDaggersInfo.Core.Asset;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.Client.Pages.Wiki;

public partial class AssetsPage
{
	private IReadOnlyList<AudioAssetInfo> _audioAudio = AudioAudio.All;
	private IReadOnlyList<MeshAssetInfo> _ddMeshes = DdMeshes.All;
	private IReadOnlyList<ObjectBindingAssetInfo> _ddObjectBindings = DdObjectBindings.All;
	private IReadOnlyList<ShaderAssetInfo> _coreShaders = CoreShaders.All;
	private IReadOnlyList<ShaderAssetInfo> _ddShaders = DdShaders.All;
	private IReadOnlyList<TextureAssetInfo> _ddTextures = DdTextures.All;

	private readonly Dictionary<string, bool> _audioAudioSortings = new();
	private readonly Dictionary<string, bool> _ddMeshesSortings = new();
	private readonly Dictionary<string, bool> _ddObjectBindingsSortings = new();
	private readonly Dictionary<string, bool> _coreShadersSortings = new();
	private readonly Dictionary<string, bool> _ddShadersSortings = new();
	private readonly Dictionary<string, bool> _ddTexturesSortings = new();

	private static void Sort<TSource, TKey>(ref IReadOnlyList<TSource> source, IDictionary<string, bool> sortings, Func<TSource, TKey> sorting, [CallerArgumentExpression("sorting")] string sortingExpression = "")
	{
		if (!sortings.TryGetValue(sortingExpression, out bool sortDirection))
			sortings.Add(sortingExpression, false);

		source = source.OrderBy(sorting, sortDirection).ToList();

		sortings[sortingExpression] = !sortDirection;
	}
}
