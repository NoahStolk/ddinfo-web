using Warp.Content;
using Warp.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.Rendering.Data;

public readonly record struct Sprite(Vector2i<int> Scale, Vector2i<int> CenterPosition, float Depth, Texture Texture, Color Color);
