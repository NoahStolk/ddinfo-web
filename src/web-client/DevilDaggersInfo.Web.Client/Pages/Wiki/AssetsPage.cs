using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Asset;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.Client.Pages.Wiki;

public partial class AssetsPage
{
	private List<AudioAssetInfo> _audioAudio = AudioAudio.All;
	private List<MeshAssetInfo> _ddMeshes = DdMeshes.All;
	private List<ObjectBindingAssetInfo> _ddObjectBindings = DdObjectBindings.All;
	private List<ShaderAssetInfo> _coreShaders = CoreShaders.All;
	private List<ShaderAssetInfo> _ddShaders = DdShaders.All;
	private List<TextureAssetInfo> _ddTextures = DdTextures.All;

	private readonly Dictionary<string, bool> _audioAudioSortings = new();
	private readonly Dictionary<string, bool> _ddMeshesSortings = new();
	private readonly Dictionary<string, bool> _ddObjectBindingsSortings = new();
	private readonly Dictionary<string, bool> _coreShadersSortings = new();
	private readonly Dictionary<string, bool> _ddShadersSortings = new();
	private readonly Dictionary<string, bool> _ddTexturesSortings = new();

	private static void Sort<TSource, TKey>(ref List<TSource> source, Dictionary<string, bool> sortings, Func<TSource, TKey> sorting, [CallerArgumentExpression("sorting")] string sortingExpression = "")
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
