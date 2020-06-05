using DevilDaggersWebsite.Code.Docs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Pages.Wiki.Docs
{
	public class ResourceBinaryModel : PageModel
	{
		public List<BinaryField> Header { get; } = new List<BinaryField>
		{
			new BinaryField("3A68783A", "Identifier", typeof(uint)),
			new BinaryField("72673A01", "Identifier", typeof(uint)),
			new BinaryField("15340000", "Table of contents buffer length", typeof(uint))
		};

		public List<BinaryField> FirstTocEntry { get; } = new List<BinaryField>
		{
			new BinaryField("10", "Chunk type", typeof(byte)),
			new BinaryField("00", "Empty", typeof(byte)),
			new BinaryField("6465627567", "Chunk name", typeof(string)),
			new BinaryField("00", "Null terminator", typeof(byte)),
			new BinaryField("21340000", "Data start offset", typeof(uint)),
			new BinaryField("6E070000", "Data buffer length", typeof(uint)),
			new BinaryField("00000000")
		};

		public List<BinaryField> SecondTocEntry { get; } = new List<BinaryField>
		{
			new BinaryField("11", "Chunk type", typeof(byte)),
			new BinaryField("00", "Empty", typeof(byte)),
			new BinaryField("6465627567", "Chunk name", typeof(string)),
			new BinaryField("00", "Null terminator", typeof(byte)),
			new BinaryField("8F3B0000", "Data start offset", typeof(uint)),
			new BinaryField("00000000", "Data buffer length", typeof(uint)),
			new BinaryField("7BCC0557")
		};

		public List<BinaryField> ThirdTocEntry { get; } = new List<BinaryField>
		{
			new BinaryField("10", "Chunk type", typeof(byte)),
			new BinaryField("00", "Empty", typeof(byte)),
			new BinaryField("6465707468", "Chunk name", typeof(string)),
			new BinaryField("00", "Null terminator", typeof(byte)),
			new BinaryField("8F3B0000", "Data start offset", typeof(uint)),
			new BinaryField("AB010000", "Data buffer length", typeof(uint)),
			new BinaryField("00000000")
		};
	}
}