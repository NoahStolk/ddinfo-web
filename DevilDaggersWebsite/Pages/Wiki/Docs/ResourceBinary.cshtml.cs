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

		public List<BinaryField> TextureHeaderDagger { get; } = new List<BinaryField>
		{
			new BinaryField("1140"),
			new BinaryField("40000000", "Texture width in pixels", typeof(uint)),
			new BinaryField("40000000", "Texture height in pixels", typeof(uint)),
			new BinaryField("07", "Texture mipmap count", typeof(byte))
		};

		public List<BinaryField> TextureHeaderHand { get; } = new List<BinaryField>
		{
			new BinaryField("1140"),
			new BinaryField("00010000", "Texture width in pixels", typeof(uint)),
			new BinaryField("00010000", "Texture height in pixels", typeof(uint)),
			new BinaryField("09", "Texture mipmap count", typeof(byte))
		};

		public List<BinaryField> TextureHeaderSorathMask { get; } = new List<BinaryField>
		{
			new BinaryField("1140"),
			new BinaryField("F0000000", "Texture width in pixels", typeof(uint)),
			new BinaryField("F0000000", "Texture height in pixels", typeof(uint)),
			new BinaryField("08", "Texture mipmap count", typeof(byte))
		};

		public List<BinaryField> ShaderHeaderDebug { get; } = new List<BinaryField>
		{
			new BinaryField("05000000", "Shader name length", typeof(uint)),
			new BinaryField("FB020000", "Vertex shader buffer length", typeof(uint)),
			new BinaryField("62040000", "Fragment shader buffer length", typeof(uint))
		};

		public List<BinaryField> ShaderHeaderDepth { get; } = new List<BinaryField>
		{
			new BinaryField("05000000", "Shader name length", typeof(uint)),
			new BinaryField("EA000000", "Vertex shader buffer length", typeof(uint)),
			new BinaryField("B0000000", "Fragment shader buffer length", typeof(uint))
		};

		public List<BinaryField> ShaderHeaderParticle { get; } = new List<BinaryField>
		{
			new BinaryField("08000000", "Shader name length", typeof(uint)),
			new BinaryField("E8050000", "Vertex shader buffer length", typeof(uint)),
			new BinaryField("0C090000", "Fragment shader buffer length", typeof(uint))
		};

		public List<BinaryField> ModelHeaderDagger { get; } = new List<BinaryField>
		{
			new BinaryField("B4000000", "Index count", typeof(uint)),
			new BinaryField("A4000000", "Vertex count", typeof(uint)),
			new BinaryField("2001")
		};

		public List<BinaryField> ModelHeaderHand { get; } = new List<BinaryField>
		{
			new BinaryField("78030000", "Index count", typeof(uint)),
			new BinaryField("B9000000", "Vertex count", typeof(uint)),
			new BinaryField("2001")
		};

		public List<BinaryField> ModelHeaderHand2 { get; } = new List<BinaryField>
		{
			new BinaryField("A4040000", "Index count", typeof(uint)),
			new BinaryField("EE000000", "Vertex count", typeof(uint)),
			new BinaryField("2001")
		};

		public List<BinaryField> ModelFirstVertexDagger { get; } = new List<BinaryField>
		{
			new BinaryField("9A99193D", "Position X", typeof(float)),
			new BinaryField("00000000", "Position Y", typeof(float)),
			new BinaryField("075F983C", "Position Z", typeof(float)),
			new BinaryField("6E12433E", "Texture coordinate U", typeof(float)),
			new BinaryField("653B3FBF", "Texture coordinate V", typeof(float)),
			new BinaryField("6F1223BF", "Normal X", typeof(float)),
			new BinaryField("492E7F3D", "Normal Y", typeof(float)),
			new BinaryField("1158993E", "Normal Z", typeof(float))
		};
	}
}