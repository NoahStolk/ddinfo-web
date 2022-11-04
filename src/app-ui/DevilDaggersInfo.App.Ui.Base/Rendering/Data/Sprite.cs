using System.Numerics;
using Warp.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.Rendering.Data;

public readonly record struct Sprite(Vector2 Scale, Vector2 CenterPosition, float Depth, Texture Texture, Color Color, Scissor? Scissor);
