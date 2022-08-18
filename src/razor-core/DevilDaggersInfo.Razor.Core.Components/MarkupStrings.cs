using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.Components;

public static class MarkupStrings
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

	public const string NoDataColor = "#666";

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

	public static readonly MarkupString Checkmark = new(@"<svg style='margin: 3px;' width='28' height='28'>
	<polyline points='3,10 10,18 20,4' style='stroke: #d00; stroke-width: 3;' />
</svg>");

	public static readonly MarkupString NoDataMarkup = new($@"<span style=""color: {NoDataColor};"">N/A</span>");

	public static readonly MarkupString HiddenMarkup = new($@"<span style=""color: {NoDataColor};"">Hidden</span>");

	public static MarkupString Space => new("&#32;");

}
