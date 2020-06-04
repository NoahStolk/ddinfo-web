using System;
using System.Globalization;

namespace DevilDaggersWebsite.Code.Docs
{
	public class BinaryField
	{
		public string HexRepresentation { get; set; }
		public string Meaning { get; set; }
		public Type Type { get; set; }

		public int FieldSize => HexRepresentation.Length / 2;

		public BinaryType BinaryType { get; }

		public BinaryField(string hexRepresentation, string meaning = null, Type type = null)
		{
			HexRepresentation = hexRepresentation;
			Meaning = meaning;
			Type = type;

			BinaryType = type != null ? BinaryTypes.Types[type] : null;
		}

		public object GetActualValue()
		{
			if (BinaryType == null)
				return null;

			return BinaryType.Converter(ConvertHexStringToByteArray(HexRepresentation));
		}

		public static byte[] ConvertHexStringToByteArray(string hexString)
		{
			if (hexString.Length % 2 != 0)
				throw new ArgumentException($"The hexadecimal string cannot have an odd {hexString.Length} number of digits: {hexString}");

			byte[] data = new byte[hexString.Length / 2];
			for (int index = 0; index < data.Length; index++)
			{
				string byteValue = hexString.Substring(index * 2, 2);
				data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}

			return data;
		}
	}
}