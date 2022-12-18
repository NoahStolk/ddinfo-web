namespace DevilDaggersInfo.Core.Mod;

public sealed class ModBinaryReadFilter
{
	private readonly bool _all;
	private readonly AssetKey[] _keys;

	private ModBinaryReadFilter(bool all, AssetKey[] keys)
	{
		_all = all;
		_keys = keys;
	}

	public static ModBinaryReadFilter AllAssets => new(true, Array.Empty<AssetKey>());
	public static ModBinaryReadFilter NoAssets => new(false, Array.Empty<AssetKey>());

	public static ModBinaryReadFilter Assets(params AssetKey[] keys) => new(false, keys);

	public bool ShouldRead(AssetKey key)
	{
		return _all || _keys.Contains(key);
	}
};
