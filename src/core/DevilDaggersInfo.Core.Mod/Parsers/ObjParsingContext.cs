using System.Globalization;

namespace DevilDaggersInfo.Core.Mod.Parsers;

public class ObjParsingContext
{
	private readonly List<Vector3> _positions = new();
	private readonly List<Vector2> _texCoords = new();
	private readonly List<Vector3> _normals = new();
	private readonly List<VertexReference> _vertices = new();

	private static float ParseVertexValue(string value)
		=> (float)double.Parse(value, NumberStyles.Float);

	public ParsedObjData Parse(string objText)
	{
		_positions.Clear();
		_texCoords.Clear();
		_normals.Clear();
		_vertices.Clear();

		string[] lines = objText.Split(Environment.NewLine);

		for (int i = 0; i < lines.Length; i++)
		{
			int lineNumber = i + 1;

			string line = lines[i].Trim();
			string[] values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (values.Length == 0)
				continue;

			string[] data = values[1..];
			switch (values[0])
			{
				case "v": ParsePosition(lineNumber, data); break;
				case "vt": ParseTexCoord(lineNumber, data); break;
				case "vn": ParseNormal(lineNumber, data); break;
				case "f": ParseFace(lineNumber, data); break;
			}
		}

		return DuplicateVertices();
	}

	private void ParsePosition(int lineNumber, string[] coords)
	{
		if (coords.Length < 3)
			throw new InvalidObjException($"Invalid position (v) on line {lineNumber}. Must contain at least 3 coordinates. (Additional coordinates are ignored.)");

		_positions.Add(new(ParseVertexValue(coords[0]), ParseVertexValue(coords[1]), ParseVertexValue(coords[2])));
	}

	private void ParseTexCoord(int lineNumber, string[] coords)
	{
		if (coords.Length < 2)
			throw new InvalidObjException($"Invalid texture coordinate (vt) on line {lineNumber}. Must contain at least 2 coordinates. (Additional coordinates are ignored.)");

		_texCoords.Add(new(ParseVertexValue(coords[0]), ParseVertexValue(coords[1])));
	}

	private void ParseNormal(int lineNumber, string[] coords)
	{
		if (coords.Length < 3)
			throw new InvalidObjException($"Invalid normal (vn) on line {lineNumber}. Must contain at least 3 coordinates. (Additional coordinates are ignored.)");

		_normals.Add(new(ParseVertexValue(coords[0]), ParseVertexValue(coords[1]), ParseVertexValue(coords[2])));
	}

	/// <summary>
	/// Parses a line that starts with "f" (face).
	/// <para>
	/// Compatible with:
	/// </para>
	/// <list type="bullet">
	/// <item>f 1 2 3</item>
	/// <item>f 1/2/3 4/5/6 7/8/9</item>
	/// </list>
	/// </summary>
	private void ParseFace(int lineNumber, string[] coords)
	{
		if (coords.Length < 3)
			throw new NotSupportedException($"Invalid face on line {lineNumber}. Must be a complete triangle.");

		if (coords.Length > 3)
			throw new NotSupportedException($"Invalid face on line {lineNumber}. Quads and NGons are not supported. Export your meshes as triangles.");

		for (int j = 0; j < 3; j++)
		{
			string value = coords[j];

			string baseErrorMessage = $"Invalid vertex data on line {lineNumber}:";

			if (value.Contains('/'))
			{
				// f 1/2/3 4/5/6 7/8/9
				string[] references = value.Split('/');

				if (references.Length != 3)
					throw new InvalidObjException($"Invalid face data on line {lineNumber}. Must contain reference to position, texture (UV), and normal coordinates.");

				if (string.IsNullOrWhiteSpace(references[0]))
					throw new InvalidObjException($"{baseErrorMessage} Empty position value found. This probably means your model file is corrupted.");
				if (string.IsNullOrWhiteSpace(references[1]))
					throw new InvalidObjException($"{baseErrorMessage} Empty texture coordinate value found. Make sure to export your texture (UV) coordinates.");
				if (string.IsNullOrWhiteSpace(references[2]))
					throw new InvalidObjException($"{baseErrorMessage} Empty normal value found. Make sure to export your normals.");

				if (!int.TryParse(references[0], out int positionReference) || positionReference < 1)
					throw new InvalidObjException($"{baseErrorMessage} Position value '{references[0]}' could not be parsed to a positive integral value.");
				if (!int.TryParse(references[1], out int texCoordReference) || texCoordReference < 1)
					throw new InvalidObjException($"{baseErrorMessage} Texture coordinate value '{references[1]}' could not be parsed to a positive integral value.");
				if (!int.TryParse(references[2], out int normalReference) || normalReference < 1)
					throw new InvalidObjException($"{baseErrorMessage} Normal value '{references[2]}' could not be parsed to a positive integral value.");

				_vertices.Add(new(positionReference, texCoordReference, normalReference));
			}
			else
			{
				// f 1 2 3
				if (string.IsNullOrWhiteSpace(value))
					throw new InvalidObjException($"{baseErrorMessage} No vertex value found. This probably means your model file is corrupted.");
				if (!int.TryParse(value, out int unifiedValue) || unifiedValue < 1)
					throw new InvalidObjException($"{baseErrorMessage} Value '{value}' could not be parsed to a positive integral value.");

				_vertices.Add(new(unifiedValue));
			}
		}
	}

	private ParsedObjData DuplicateVertices()
	{
		ParsedObjData parsed = new();

		for (int i = 0; i < _vertices.Count; i += 3)
		{
			VertexReference a = _vertices[i];
			VertexReference b = _vertices[i + 1];
			VertexReference c = _vertices[i + 2];

			if (_positions.Count < a.PositionReference)
				throw new InvalidObjException($"Face vertex A targets position {a.PositionReference} but there are only {_positions.Count} positions.");
			if (_positions.Count < b.PositionReference)
				throw new InvalidObjException($"Face vertex B targets position {b.PositionReference} but there are only {_positions.Count} positions.");
			if (_positions.Count < c.PositionReference)
				throw new InvalidObjException($"Face vertex C targets position {c.PositionReference} but there are only {_positions.Count} positions.");

			parsed.Positions.Add(_positions[a.PositionReference - 1]);
			parsed.Positions.Add(_positions[b.PositionReference - 1]);
			parsed.Positions.Add(_positions[c.PositionReference - 1]);

			if (_texCoords.Count < a.TexCoordReference)
				throw new InvalidObjException($"Face vertex A targets texture coordinate {a.TexCoordReference} but there are only {_texCoords.Count} texture coordinates.");
			if (_texCoords.Count < b.TexCoordReference)
				throw new InvalidObjException($"Face vertex B targets texture coordinate {b.TexCoordReference} but there are only {_texCoords.Count} texture coordinates.");
			if (_texCoords.Count < c.TexCoordReference)
				throw new InvalidObjException($"Face vertex C targets texture coordinate {c.TexCoordReference} but there are only {_texCoords.Count} texture coordinates.");

			parsed.TexCoords.Add(_texCoords[a.TexCoordReference - 1]);
			parsed.TexCoords.Add(_texCoords[b.TexCoordReference - 1]);
			parsed.TexCoords.Add(_texCoords[c.TexCoordReference - 1]);

			if (_normals.Count < a.NormalReference)
				throw new InvalidObjException($"Face vertex A targets normal {a.NormalReference} but there are only {_normals.Count} normals.");
			if (_normals.Count < b.NormalReference)
				throw new InvalidObjException($"Face vertex B targets normal {b.NormalReference} but there are only {_normals.Count} normals.");
			if (_normals.Count < c.NormalReference)
				throw new InvalidObjException($"Face vertex C targets normal {c.NormalReference} but there are only {_normals.Count} normals.");

			parsed.Normals.Add(_normals[a.NormalReference - 1]);
			parsed.Normals.Add(_normals[b.NormalReference - 1]);
			parsed.Normals.Add(_normals[c.NormalReference - 1]);

			parsed.Vertices.Add(new(i + 1));
			parsed.Vertices.Add(new(i + 2));
			parsed.Vertices.Add(new(i + 3));
		}

		return parsed;
	}
}
