using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;

string inputPath = args[0];
string outputDirectory = args[1];

string fileName = Path.GetFileName(inputPath);
byte[] fileContents = File.ReadAllBytes(inputPath);
ModBinary modBinary = new(fileName, fileContents, ModBinaryReadComprehensiveness.All);
modBinary.ExtractAssets(outputDirectory);
