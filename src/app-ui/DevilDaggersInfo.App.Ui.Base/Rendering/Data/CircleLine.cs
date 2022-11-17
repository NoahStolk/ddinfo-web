using Warp.NET.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.Rendering.Data;

public readonly record struct CircleLine(Vector2i<int> CenterPosition, float Radius, float Depth, Color Color, Scissor? Scissor);
