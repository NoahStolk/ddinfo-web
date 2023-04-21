using System.Collections.Generic;

namespace Warp.NET.Parsers.Model;

public record MeshData(string MaterialName, IReadOnlyList<Face> Faces);
