using DevilDaggersInfo.Razor.Core.Canvas.JS;
using System.Runtime.InteropServices.JavaScript;

namespace DevilDaggersInfo.Razor.Core.Canvas;

public partial class WebAssemblyCanvas2d : Canvas
{
	public const string ModuleName = nameof(WebAssemblyCanvas2d);

	public WebAssemblyCanvas2d(string id)
		: base(id)
	{
	}

	public string FillStyle
	{
		get => GetFillStyle(Id);
		set => SetFillStyle(Id, value);
	}

	public string StrokeStyle
	{
		get => GetStrokeStyle(Id);
		set => SetStrokeStyle(Id, value);
	}

	public string Font
	{
		get => GetFont(Id);
		set => SetFont(Id, value);
	}

	public TextAlign TextAlign
	{
		get
		{
			Enum.TryParse(GetTextAlign(Id), true, out TextAlign textAlign);
			return textAlign;
		}
		set => SetTextAlign(Id, value.ToString().ToLower());
	}

	public double LineWidth
	{
		// string str = GetLineWidth(Id);
		// _ = double.TryParse(str, out double width);
		// _lineWidth = width;
		get => GetLineWidth(Id);
		set => SetLineWidth(Id, value);
	}

	public void ClearRect(double x, double y, double width, double height)
		=> ClearRect(Id, x, y, width, height);

	public void FillRect(double x, double y, double width, double height)
		=> FillRect(Id, x, y, width, height);

	public void StrokeRect(double x, double y, double width, double height)
		=> StrokeRect(Id, x, y, width, height);

	public void BeginPath()
		=> BeginPath(Id);

	public void ClosePath()
		=> ClosePath(Id);

	public void Stroke()
		=> Stroke(Id);

	public void Fill()
		=> Fill(Id);

	public void Circle(double x, double y, double radius)
		=> Arc(Id, x, y, radius, 0.0f, MathF.PI * 2);

	public void MoveTo(double x, double y)
		=> MoveTo(Id, x, y);

	public void LineTo(double x, double y)
		=> LineTo(Id, x, y);

	public void Save()
		=> Save(Id);

	public void Restore()
		=> Restore(Id);

	public void Translate(double x, double y)
		=> Translate(Id, x, y);

	public void Rotate(double angle)
		=> Rotate(Id, angle);

	public void StrokeText(string text, double x, double y)
		=> StrokeText(Id, text, x, y);

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
	private static partial double GetLineWidth(string id);

	[JSImport("setLineWidth", ModuleName)]
	private static partial void SetLineWidth(string id, double lineWidth);

	[JSImport("clearRect", ModuleName)]
	private static partial void ClearRect(string id, double x, double y, double width, double height);

	[JSImport("fillRect", ModuleName)]
	private static partial void FillRect(string id, double x, double y, double width, double height);

	[JSImport("strokeRect", ModuleName)]
	private static partial void StrokeRect(string id, double x, double y, double width, double height);

	[JSImport("beginPath", ModuleName)]
	private static partial void BeginPath(string id);

	[JSImport("closePath", ModuleName)]
	private static partial void ClosePath(string id);

	[JSImport("moveTo", ModuleName)]
	private static partial void MoveTo(string id, double x, double y);

	[JSImport("lineTo", ModuleName)]
	private static partial void LineTo(string id, double x, double y);

	[JSImport("bezierCurveTo", ModuleName)]
	private static partial void BezierCurveTo(string id, double cp1X, double cp1Y, double cp2X, double cp2Y, double x, double y);

	[JSImport("arc", ModuleName)]
	private static partial void Arc(string id, double x, double y, double radius, double startAngle, double endAngle, bool anticlockwise = false);

	[JSImport("arcTo", ModuleName)]
	private static partial void ArcTo(string id, double x1, double y1, double x2, double y2, double radius);

	[JSImport("rect", ModuleName)]
	private static partial void Rect(string id, double x, double y, double width, double height);

	[JSImport("ellipse", ModuleName)]
	private static partial void Ellipse(string id, double x, double y, double radiusX, double radiusY, double rotation = 0f, double startAngle = 0f, double endAngle = MathF.PI * 2, bool anticlockwise = false);

	[JSImport("fill", ModuleName)]
	private static partial void Fill(string id);

	[JSImport("stroke", ModuleName)]
	private static partial void Stroke(string id);

	[JSImport("rotate", ModuleName)]
	private static partial void Rotate(string id, double angle);

	[JSImport("rotateAt", ModuleName)]
	private static partial void RotateAt(string id, double x, double y, double angle);

	[JSImport("scale", ModuleName)]
	private static partial void Scale(string id, double x, double y);

	[JSImport("translate", ModuleName)]
	private static partial void Translate(string id, double x, double y);

	[JSImport("transform", ModuleName)]
	private static partial void Transform(string id, double m11, double m12, double m21, double m22, double dx, double dy);

	[JSImport("setTransform", ModuleName)]
	private static partial void SetTransform(string id, double m11, double m12, double m21, double m22, double dx, double dy);

	[JSImport("save", ModuleName)]
	private static partial void Save(string id);

	[JSImport("restore", ModuleName)]
	private static partial void Restore(string id);

	[JSImport("strokeText", ModuleName)]
	private static partial void StrokeText(string id, string text, double x, double y);

	#endregion JSImport
}
