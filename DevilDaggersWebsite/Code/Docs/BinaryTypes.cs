using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.Docs
{
	public static class BinaryTypes
	{
		public static readonly Dictionary<Type, BinaryType> Types = new Dictionary<Type, BinaryType>
		{
			{ typeof(double), new BinaryType("64-bit float", (bytes) => BitConverter.ToDouble(bytes)) },
			{ typeof(float), new BinaryType("32-bit float", (bytes) => BitConverter.ToSingle(bytes)) },
			{ typeof(ulong), new BinaryType("64-bit unsigned integer", (bytes) => BitConverter.ToUInt64(bytes)) },
			{ typeof(uint), new BinaryType("32-bit unsigned integer", (bytes) => BitConverter.ToUInt32(bytes)) },
			{ typeof(ushort), new BinaryType("16-bit unsigned integer", (bytes) => BitConverter.ToUInt16(bytes)) },
			{ typeof(byte), new BinaryType("8-bit unsigned integer", (bytes) => bytes[0]) },
			{ typeof(long), new BinaryType("64-bit signed integer", (bytes) => BitConverter.ToInt64(bytes)) },
			{ typeof(int), new BinaryType("32-bit signed integer", (bytes) => BitConverter.ToInt32(bytes)) },
			{ typeof(short), new BinaryType("16-bit signed integer", (bytes) => BitConverter.ToInt16(bytes)) }
		};
	}
}