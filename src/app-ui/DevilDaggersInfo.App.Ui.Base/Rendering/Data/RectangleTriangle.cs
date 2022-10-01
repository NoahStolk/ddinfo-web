using Warp.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.Rendering.Data;

public readonly record struct RectangleTriangle(Vector2i<int> Scale, Vector2i<int> CenterPosition, float Depth, Color Color);
