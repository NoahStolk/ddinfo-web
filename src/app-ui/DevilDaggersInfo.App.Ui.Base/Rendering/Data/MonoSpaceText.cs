using DevilDaggersInfo.App.Ui.Base.Enums;
using Warp.NET.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.Rendering.Data;

public readonly record struct MonoSpaceText(Vector2i<int> Scale, Vector2i<int> Position, float Depth, Color Color, string Text, TextAlign TextAlign, Scissor? Scissor);
