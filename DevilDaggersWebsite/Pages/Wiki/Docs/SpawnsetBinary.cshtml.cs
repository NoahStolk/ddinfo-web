using DevilDaggersWebsite.Code.Docs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Pages.Wiki.Docs
{
	public class SpawnsetBinaryModel : PageModel
	{
		public List<BinaryField> Header { get; } = new List<BinaryField>
		{
			new BinaryField("04000000"),
			new BinaryField("09000000"),
			new BinaryField("0000A041", "Shrink end", typeof(float)),
			new BinaryField("01004842", "Shrink start", typeof(float)),
			new BinaryField("CDCCCC3C", "Shrink rate", typeof(float)),
			new BinaryField("00007042", "Brightness", typeof(float)),
			new BinaryField("00000000"),
			new BinaryField("33000000"),
			new BinaryField("01000000"),
		};

		public List<BinaryField> SpawnsHeader { get; } = new List<BinaryField>
		{
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("01000000"),
			new BinaryField("F4010000"),
			new BinaryField("FA000000"),
			new BinaryField("78000000"),
			new BinaryField("3C000000"),
			new BinaryField("00000000"),
			new BinaryField("76000000", "Spawn count", typeof(uint)),
		};

		public List<BinaryField> FirstSpawn { get; } = new List<BinaryField>
		{
			new BinaryField("00000000", "Enemy type", typeof(int)),
			new BinaryField("00004040", "Spawn delay", typeof(float)),
			new BinaryField("00000000"),
			new BinaryField("03000000"),
			new BinaryField("00000000"),
			new BinaryField("0000F041"),
			new BinaryField("0A000000"),
		};

		public List<BinaryField> SecondSpawn { get; } = new List<BinaryField>
		{
			new BinaryField("FFFFFFFF", "Enemy type", typeof(int)),
			new BinaryField("0000C040", "Spawn delay", typeof(float)),
			new BinaryField("00000000"),
			new BinaryField("03000000"),
			new BinaryField("00000000"),
			new BinaryField("0000F041"),
			new BinaryField("0A000000"),
		};

		public List<BinaryField> ThirdSpawn { get; } = new List<BinaryField>
		{
			new BinaryField("00000000", "Enemy type", typeof(int)),
			new BinaryField("0000A040", "Spawn delay", typeof(float)),
			new BinaryField("00000000"),
			new BinaryField("03000000"),
			new BinaryField("00000000"),
			new BinaryField("0000F041"),
			new BinaryField("0A000000"),
		};
	}
}