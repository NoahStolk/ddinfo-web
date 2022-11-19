using DevilDaggersInfo.Razor.Core.Canvas.JS;
using System.Runtime.InteropServices.JavaScript;

namespace DevilDaggersInfo.Razor.Core.Canvas;

public partial class WebAssemblyCanvas2d : Canvas
{
	public const string ModuleName = nameof(WebAssemblyCanvas2d);

	private string? _fillStyle;
	private string? _strokeStyle;
	private string? _font;
	private TextAlign? _textAlign;
	private float? _lineWidth;

	public WebAssemblyCanvas2d(string id)
		: base(id)
	{
	}

	public string FillStyle
	{
		get => _fillStyle ??= GetFillStyle(Id);
		set
		{
			if (_fillStyle == value)
				return;

			_fillStyle = value;
			SetFillStyle(Id, value);
		}
	}

	public string StrokeStyle
	{
		get => _strokeStyle ??= GetStrokeStyle(Id);
		set
		{
			if (_strokeStyle == value)
				return;

			_strokeStyle = value;
			SetStrokeStyle(Id, value);
		}
	}

	public string Font
	{
		get => _font ??= GetFont(Id);
		set
		{
			if (_font == value)
				return;

			_font = value;
			SetFont(Id, value);
		}
	}

	public TextAlign TextAlign
	{
		get
		{
			if (_textAlign == null)
			{
				string str = GetTextAlign(Id);
				Enum.TryParse(str, true, out TextAlign ret);
				_textAlign = ret;
			}

			return _textAlign.Value;
		}
		set
		{
			if (_textAlign == value)
				return;

			_textAlign = value;
			SetTextAlign(Id, value.ToString().ToLower());
		}
	}

	public float LineWidth
	{
		get
		{
			if (_lineWidth == null)
			{
				// string str = GetLineWidth(Id);
				// _ = float.TryParse(str, out float width);
				// _lineWidth = width;
				_lineWidth = GetLineWidth(Id);
			}

			return _lineWidth.Value;
		}
		set
		{
			if (_lineWidth == value)
				return;

			_lineWidth = value;
			SetLineWidth(Id, value);
		}
	}

	public void ClearRect(float x, float y, float width, float height)
		=> ClearRect(Id, x, y, width, height);

	public void FillRect(float x, float y, float width, float height)
		=> FillRect(Id, x, y, width, height);

	public void StrokeRect(float x, float y, float width, float height)
		=> StrokeRect(Id, x, y, width, height);

	public void BeginPath()
		=> BeginPath(Id);

	public void ClosePath()
		=> ClosePath(Id);

	public void Stroke()
		=> Stroke(Id);

	public void Fill()
		=> Fill(Id);

	public void Circle(float x, float y, float radius)
		=> Arc(Id, x, y, radius, 0.0f, MathF.PI * 2);

	public void MoveTo(float x, float y)
		=> MoveTo(Id, x, y);

	public void LineTo(float x, float y)
		=> LineTo(Id, x, y);

	#region JSImport

	[JSImport("getFillStyle", ModuleName)]
	private static partial string GetFillStyle(string id);

	[JSImport("setFillStyle", ModuleName)]
	private static partial void SetFillStyle(string id, string fillStyle);

	[JSImport("getStrokeStyle", ModuleName)]
	private static partial string GetStrokeStyle(string id);

	[JSImport("setStrokeStyle", ModuleName)]
	private static partial void SetStrokeStyle(string id, string strokeStyle);

	[JSImport("getFont", ModuleName)]
	private static partial string GetFont(string id);

	[JSImport("setFont", ModuleName)]
	private static partial void SetFont(string id, string font);

	[JSImport("getTextAlign", ModuleName)]
	private static partial string GetTextAlign(string id);

	[JSImport("setTextAlign", ModuleName)]
	private static partial void SetTextAlign(string id, string textAlign);

	[JSImport("getLineWidth", ModuleName)]
	private static partial float GetLineWidth(string id);

	[JSImport("setLineWidth", ModuleName)]
	private static partial void SetLineWidth(string id, float lineWidth);

	[JSImport("clearRect", ModuleName)]
	private static partial void ClearRect(string id, float x, float y, float width, float height);

	[JSImport("fillRect", ModuleName)]
	private static partial void FillRect(string id, float x, float y, float width, float height);

	[JSImport("strokeRect", ModuleName)]
	private static partial void StrokeRect(string id, float x, float y, float width, float height);

	[JSImport("beginPath", ModuleName)]
	private static partial void BeginPath(string id);

	[JSImport("closePath", ModuleName)]
	private static partial void ClosePath(string id);

	[JSImport("moveTo", ModuleName)]
	private static partial void MoveTo(string id, float x, float y);

	[JSImport("lineTo", ModuleName)]
	private static partial void LineTo(string id, float x, float y);

	[JSImport("bezierCurveTo", ModuleName)]
	private static partial void BezierCurveTo(string id, float cp1X, float cp1Y, float cp2X, float cp2Y, float x, float y);

	[JSImport("arc", ModuleName)]
	private static partial void Arc(string id, float x, float y, float radius, float startAngle, float endAngle, bool anticlockwise = false);

	[JSImport("arcTo", ModuleName)]
	private static partial void ArcTo(string id, float x1, float y1, float x2, float y2, float radius);

	[JSImport("rect", ModuleName)]
	private static partial void Rect(string id, float x, float y, float width, float height);

	[JSImport("ellipse", ModuleName)]
	private static partial void Ellipse(string id, float x, float y, float radiusX, float radiusY, float rotation = 0f, float startAngle = 0f, float endAngle = MathF.PI * 2, bool anticlockwise = false);

	[JSImport("fill", ModuleName)]
	private static partial void Fill(string id);

	[JSImport("stroke", ModuleName)]
	private static partial void Stroke(string id);

	[JSImport("rotate", ModuleName)]
	private static partial void Rotate(string id, float angle);

	[JSImport("rotateAt", ModuleName)]
	private static partial void RotateAt(string id, float x, float y, float angle);

	[JSImport("scale", ModuleName)]
	private static partial void Scale(string id, float x, float y);

	[JSImport("translate", ModuleName)]
	private static partial void Translate(string id, float x, float y);

	[JSImport("transform", ModuleName)]
	private static partial void Transform(string id, float m11, float m12, float m21, float m22, float dx, float dy);

	[JSImport("setTransform", ModuleName)]
	private static partial void SetTransform(string id, float m11, float m12, float m21, float m22, float dx, float dy);

	[JSImport("save", ModuleName)]
	private static partial void Save(string id);

	[JSImport("restore", ModuleName)]
	private static partial void Restore(string id);

	#endregion JSImport
}
