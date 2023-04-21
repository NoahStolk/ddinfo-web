#pragma warning disable SA1201 // Elements should appear in the correct order
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable S1104 // Fields should not have public accessibility
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Warp.NET.Maths.Numerics;

public struct Vector3i<T> : IEquatable<Vector3i<T>>, IFormattable
	where T : IBinaryInteger<T>, IMinMaxValue<T>
{
	public T X;

	public T Y;

	public T Z;

	public Vector3i(T value)
		: this(value, value, value)
	{
	}

	public Vector3i(T x, T y, T z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public static Vector3i<T> Zero => default;

	public static Vector3i<T> One => new(T.One);

	public static Vector3i<T> UnitX => new(T.One, T.Zero, T.Zero);

	public static Vector3i<T> UnitY => new(T.Zero, T.One, T.Zero);

	public static Vector3i<T> UnitZ => new(T.Zero, T.Zero, T.One);

	#region Equality

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Vector3i<T> left, Vector3i<T> right)
		=> left.X == right.X && left.Y == right.Y && left.Z == right.Z;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector3i<T> left, Vector3i<T> right)
		=> !(left == right);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override readonly bool Equals([NotNullWhen(true)] object? obj)
		=> obj is Vector3i<T> other && Equals(other);

	public readonly bool Equals(Vector3i<T> other)
		=> this == other;

	public override readonly int GetHashCode()
		=> HashCode.Combine(X, Y, Z);

	#endregion Equality

	#region Addition

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> operator +(Vector3i<T> left, Vector3i<T> right)
		=> new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

	#endregion Addition

	#region Subtraction

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> operator -(Vector3i<T> left, Vector3i<T> right)
		=> new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

	#endregion Subtraction

	#region Multiplication

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> operator *(Vector3i<T> left, Vector3i<T> right)
		=> new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> operator *(Vector3i<T> left, T right)
		=> left * new Vector3i<T>(right);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> operator *(T left, Vector3i<T> right)
		=> right * left;

	#endregion Multiplication

	#region Division

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> operator /(Vector3i<T> left, Vector3i<T> right)
		=> new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> operator /(Vector3i<T> value1, T value2)
		=> value1 / new Vector3i<T>(value2);

	#endregion Division

	#region Negation

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> operator -(Vector3i<T> value)
		=> Zero - value;

	#endregion Negation

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> Max(Vector3i<T> value1, Vector3i<T> value2)
	{
		return new(
			value1.X > value2.X ? value1.X : value2.X,
			value1.Y > value2.Y ? value1.Y : value2.Y,
			value1.Z > value2.Z ? value1.Z : value2.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> Min(Vector3i<T> value1, Vector3i<T> value2)
	{
		return new(
			value1.X < value2.X ? value1.X : value2.X,
			value1.Y < value2.Y ? value1.Y : value2.Y,
			value1.Z < value2.Z ? value1.Z : value2.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> Abs(Vector3i<T> value)
		=> new(T.Abs(value.X), T.Abs(value.Y), T.Abs(value.Z));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i<T> Clamp(Vector3i<T> value1, Vector3i<T> min, Vector3i<T> max)
		=> Min(Max(value1, min), max);

	public override readonly string ToString()
		=> ToString("G", CultureInfo.CurrentCulture);

	public readonly string ToString(string? format)
		=> ToString(format, CultureInfo.CurrentCulture);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
	{
		StringBuilder sb = new();
		string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
		sb.Append('<');
		sb.Append(X.ToString(format, formatProvider));
		sb.Append(separator);
		sb.Append(' ');
		sb.Append(Y.ToString(format, formatProvider));
		sb.Append(separator);
		sb.Append(' ');
		sb.Append(Z.ToString(format, formatProvider));
		sb.Append('>');
		return sb.ToString();
	}
}
