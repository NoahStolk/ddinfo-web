using DevilDaggersInfo.Web.Client.Core.Canvas.JS;
using System.Runtime.InteropServices.JavaScript;

namespace DevilDaggersInfo.Web.Client.Core.Canvas;

public partial class WebAssemblyCanvas2d : Canvas
{
	private const string _moduleName = nameof(WebAssemblyCanvas2d);

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

	[JSImport("getFillStyle", _moduleName)]
	private static partial string GetFillStyle(string id);

	[JSImport("setFillStyle", _moduleName)]
	private static partial void SetFillStyle(string id, string fillStyle);

	[JSImport("getStrokeStyle", _moduleName)]
	private static partial string GetStrokeStyle(string id);

	[JSImport("setStrokeStyle", _moduleName)]
	private static partial void SetStrokeStyle(string id, string strokeStyle);

	[JSImport("getFont", _moduleName)]
	private static partial string GetFont(string id);

	[JSImport("setFont", _moduleName)]
	private static partial void SetFont(string id, string font);

	[JSImport("getTextAlign", _moduleName)]
	private static partial string GetTextAlign(string id);

	[JSImport("setTextAlign", _moduleName)]
	private static partial void SetTextAlign(string id, string textAlign);

	[JSImport("getLineWidth", _moduleName)]
	private static partial double GetLineWidth(string id);

	[JSImport("setLineWidth", _moduleName)]
	private static partial void SetLineWidth(string id, double lineWidth);

	[JSImport("clearRect", _moduleName)]
	private static partial void ClearRect(string id, double x, double y, double width, double height);

	[JSImport("fillRect", _moduleName)]
	private static partial void FillRect(string id, double x, double y, double width, double height);

	[JSImport("strokeRect", _moduleName)]
	private static partial void StrokeRect(string id, double x, double y, double width, double height);

	[JSImport("beginPath", _moduleName)]
	private static partial void BeginPath(string id);

	[JSImport("closePath", _moduleName)]
	private static partial void ClosePath(string id);

	[JSImport("moveTo", _moduleName)]
	private static partial void MoveTo(string id, double x, double y);

	[JSImport("lineTo", _moduleName)]
	private static partial void LineTo(string id, double x, double y);

	[JSImport("bezierCurveTo", _moduleName)]
	private static partial void BezierCurveTo(string id, double cp1X, double cp1Y, double cp2X, double cp2Y, double x, double y);

	[JSImport("arc", _moduleName)]
	private static partial void Arc(string id, double x, double y, double radius, double startAngle, double endAngle, bool anticlockwise = false);

	[JSImport("arcTo", _moduleName)]
	private static partial void ArcTo(string id, double x1, double y1, double x2, double y2, double radius);

	[JSImport("rect", _moduleName)]
	private static partial void Rect(string id, double x, double y, double width, double height);

	[JSImport("ellipse", _moduleName)]
	private static partial void Ellipse(string id, double x, double y, double radiusX, double radiusY, double rotation = 0f, double startAngle = 0f, double endAngle = MathF.PI * 2, bool anticlockwise = false);

	[JSImport("fill", _moduleName)]
	private static partial void Fill(string id);

	[JSImport("stroke", _moduleName)]
	private static partial void Stroke(string id);

	[JSImport("rotate", _moduleName)]
	private static partial void Rotate(string id, double angle);

	[JSImport("rotateAt", _moduleName)]
	private static partial void RotateAt(string id, double x, double y, double angle);

	[JSImport("scale", _moduleName)]
	private static partial void Scale(string id, double x, double y);

	[JSImport("translate", _moduleName)]
	private static partial void Translate(string id, double x, double y);

	[JSImport("transform", _moduleName)]
	private static partial void Transform(string id, double m11, double m12, double m21, double m22, double dx, double dy);

	[JSImport("setTransform", _moduleName)]
	private static partial void SetTransform(string id, double m11, double m12, double m21, double m22, double dx, double dy);

	[JSImport("save", _moduleName)]
	private static partial void Save(string id);

	[JSImport("restore", _moduleName)]
	private static partial void Restore(string id);

	[JSImport("strokeText", _moduleName)]
	private static partial void StrokeText(string id, string text, double x, double y);

	#endregion JSImport
}
