using System;

namespace DevilDaggersWebsite.Code.Docs
{
	public class BinaryType
	{
		public string Description { get; set; }
		public Func<byte[], object> Converter { get; set; }

		public BinaryType(string description, Func<byte[], object> converter)
		{
			Description = description;
			Converter = converter;
		}
	}
}