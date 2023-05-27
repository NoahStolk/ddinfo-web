using DevilDaggersInfo.Core.Mod.Exceptions;
using DevilDaggersInfo.Core.Mod.Parsers;

namespace DevilDaggersInfo.Core.Mod.Test;

[TestClass]
public class ObjParseTests
{
	[DataTestMethod]
	[DataRow("3dsMax-claw.obj", 430, 368, 430, 784)]
	[DataRow("3dsMax-hand.obj", 274, 329, 274, 528)]
	[DataRow("Wings3d-cylinder.obj", 32, 32, 32, 60)]
	[DataRow("Wings3d-cube.obj", 8, 8, 8, 12)]
	public void ParseObj(string fileName, int sourcePositionCount, int sourceTexCoordCount, int sourceNormalCount, int sourceFaceCount)
	{
		ObjParsingContext parser = new();
		string objText = File.ReadAllText(Path.Combine("Resources", "Mesh", fileName));
		ParsedObjData parsed = parser.Parse(objText);

		// TODO: Test counts more accurately.
		Assert.IsTrue(sourcePositionCount <= parsed.Positions.Count);
		Assert.IsTrue(sourceTexCoordCount <= parsed.TexCoords.Count);
		Assert.IsTrue(sourceNormalCount <= parsed.Normals.Count);
		Assert.IsTrue(sourceFaceCount <= parsed.Vertices.Count);
	}

	[DataTestMethod]
	[DataRow("Wings3d-cube-invalid-face.obj")]
	[DataRow("Wings3d-cube-no-uv.obj")]
	public void ParseInvalidObj(string fileName)
	{
		ObjParsingContext parser = new();
		string objText = File.ReadAllText(Path.Combine("Resources", "Mesh", fileName));
		Assert.ThrowsException<InvalidObjException>(() => parser.Parse(objText));
	}
}
