using DevilDaggersWebsite.Code.Docs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Pages.Wiki.Docs
{
	public class ParticleBinaryModel : PageModel
	{
		public List<BinaryField> Header { get; } = new List<BinaryField>
		{
			new BinaryField("04000000"),
			new BinaryField("6F000000", "Particle amount", typeof(uint))
		};

		public List<BinaryField> FirstParticleName { get; } = new List<BinaryField>
		{
			new BinaryField("626C6F6F64", "Particle name", typeof(string))
		};

		public List<BinaryField> SecondParticleName { get; } = new List<BinaryField>
		{
			new BinaryField("63656E746970656465627572726F77", "Particle name", typeof(string))
		};

		public List<BinaryField> FirstParticleData { get; } = new List<BinaryField>
		{
			new BinaryField("00000000"),
			new BinaryField("00000080"),
			new BinaryField("438FC235"),
			new BinaryField("3F52B886"),
			new BinaryField("40FFFF7F"),
			new BinaryField("3FFFFF7F"),
			new BinaryField("3FFFFF7F"),
			new BinaryField("3FFFFF7F"),
			new BinaryField("3FFFFF7F"),
			new BinaryField("33FFFF0F"),
			new BinaryField("35000080"),
			new BinaryField("3F818000"),
			new BinaryField("3E000000"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("007B142E"),
			new BinaryField("3F010129"),
			new BinaryField("5C0F3F00"),
			new BinaryField("00803F8F"),
			new BinaryField("C2F53C00"),
			new BinaryField("00504000"),
			new BinaryField("00C64100"),
			new BinaryField("00000000"),
			new BinaryField("CCCCCC3D"),
			new BinaryField("1E856B3E"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("0000803F"),
			new BinaryField("0000803F"),
			new BinaryField("0000803F"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("00002040"),
			new BinaryField("00000041"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("0AD7A33C"),
			new BinaryField("0000803F"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("00000000"),
			new BinaryField("0000803F")
		};
	}
}