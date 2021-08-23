using DevilDaggersInfo.Core.Mod;

string inputPath = args[0];
string outputDirectory = args[1];

string fileName = Path.GetFileName(inputPath);
byte[] fileContents = File.ReadAllBytes(inputPath);
ModBinary modBinary = ModBinaryHandler.ReadModBinary(fileName, fileContents);
modBinary.ExtractAssets(outputDirectory, fileContents);
