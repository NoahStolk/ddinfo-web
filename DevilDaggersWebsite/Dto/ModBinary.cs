using DevilDaggersWebsite.Enumerators;

namespace DevilDaggersWebsite.Dto
{
	public class ModBinary
	{
		public ModBinary(string name, ModBinaryType modBinaryType)
		{
			Name = name;
			ModBinaryType = modBinaryType;
		}

		public string Name { get; }
		public ModBinaryType ModBinaryType { get; }
	}
}
