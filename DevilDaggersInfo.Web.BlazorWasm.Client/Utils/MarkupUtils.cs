using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class MarkupUtils
{
	private const int _buttonSize = 40; // Hardcoded in Razor components
	private const int _margin = 10;

	private const string _fillStyle = "#ddd";

	private const float _size = _buttonSize - _margin * 2;

	private const float _a = 0;
	private const float _b = _size;
	private const float _m = _size / 2;

	private const float _am = _m / 2;
	private const float _bm = _b - _m / 2;

	private const float _lineThickness = 2;

	public static readonly MarkupString NavStart = new($@"<svg style='margin: {_margin}px;' width='{_size}' height='{_size}'>
	<rect width='{_lineThickness}' height='{_size}' x='0' y='{_a}' style='fill: {_fillStyle};' />
	<polygon points='{_a},{_m} {_m},{_a} {_m},{_b}' style='fill: {_fillStyle};' />
	<polygon points='{_m},{_m} {_b},{_a} {_b},{_b}' style='fill: {_fillStyle};' />
</svg>");
	public static readonly MarkupString NavPrevDouble = new($@"<svg style='margin: {_margin}px;' width='{_size}' height='{_size}'>
	<polygon points='{_a},{_m} {_m},{_a} {_m},{_b}' style='fill: {_fillStyle};' />
	<polygon points='{_m},{_m} {_b},{_a} {_b},{_b}' style='fill: {_fillStyle};' />
</svg>");
	public static readonly MarkupString NavPrev = new($@"<svg style='margin: {_margin}px;' width='{_size}' height='{_size}'>
	<polygon points='{_am},{_m} {_bm},{_a} {_bm},{_b}' style='fill: {_fillStyle};' />
</svg>");
	public static readonly MarkupString NavNext = new($@"<svg style='margin: {_margin}px;' width='{_size}' height='{_size}'>
	<polygon points='{_am},{_a} {_am},{_b} {_bm},{_m}' style='fill: {_fillStyle};' />
</svg>");
	public static readonly MarkupString NavNextDouble = new($@"<svg style='margin: {_margin}px;' width='{_size}' height='{_size}'>
	<polygon points='{_a},{_a} {_a},{_b} {_m},{_m}' style='fill: {_fillStyle};' />
	<polygon points='{_m},{_a} {_m},{_b} {_b},{_m}' style='fill: {_fillStyle};' />
</svg>");
	public static readonly MarkupString NavEnd = new($@"<svg style='margin: {_margin}px;' width='{_size}' height='{_size}'>
	<polygon points='{_a},{_a} {_a},{_b} {_m},{_m}' style='fill: {_fillStyle};' />
	<polygon points='{_m},{_a} {_m},{_b} {_b},{_m}' style='fill: {_fillStyle};' />
	<rect width='{_lineThickness}' height='{_size}' x='{_size - _lineThickness}' y='{_a}' style='fill: {_fillStyle};' />
</svg>");

	public static readonly MarkupString NoDataMarkup = new(@"<span style=""color: #666;"">N/A</span>");

	public static MarkupString DaggerString(Dagger dagger)
	{
		return new(@$"<span class=""font-goethe text-lg {dagger.Name.ToLower()}"">{dagger.Name} Dagger</span>");
	}

	public static MarkupString DeathString(byte deathType, GameVersion gameVersion = GameConstants.CurrentVersion, string textSizeClass = "text-lg")
	{
		Death? death = Deaths.GetDeathByLeaderboardType(gameVersion, deathType);
		return DeathString(death, textSizeClass);
	}

	public static MarkupString DeathString(Death? death, string textSizeClass = "text-lg")
	{
		string style = $"color: {death?.Color.HexCode ?? "#444"};";
		return new(@$"<span style=""{style}"" class=""font-goethe {textSizeClass}"">{death?.Name ?? "Unknown"}</span>");
	}

	public static MarkupString UpgradeString(Upgrade upgrade)
	{
		string style = $"color: {upgrade.Color.HexCode};";
		return new(@$"<span style=""{style}"" class=""font-goethe text-lg"">{upgrade.Name}</span>");
	}

	public static MarkupString EnemyString(Enemy enemy, bool plural = false)
	{
		string style = $"color: {enemy.Color.HexCode};";
		return new(@$"<span style=""{style}"" class=""font-goethe text-lg"">{enemy.Name}{(plural ? "s" : string.Empty)}</span>");
	}

	public static MarkupString Space() => new("&#32;");

	public static MarkupString LeaderboardTime(double timeInSeconds)
	{
		return new(@$"<span class=""{Daggers.GetDaggerFromSeconds(GameConstants.CurrentVersion, timeInSeconds).Name.ToLower()} font-goethe text-lg"">{timeInSeconds.ToString(FormatUtils.TimeFormat)}</span>");
	}
}
