using DevilDaggersWebsite.Enumerators;

namespace DevilDaggersWebsite.Dto
{
	public class ModBinary
	{
		public ModBinary(string name, long size, ModBinaryType modBinaryType)
		{
			Name = name;
			Size = size;
			ModBinaryType = modBinaryType;
		}

		public string Name { get; }
		public long Size { get; }
		public ModBinaryType ModBinaryType { get; }
	}
}
