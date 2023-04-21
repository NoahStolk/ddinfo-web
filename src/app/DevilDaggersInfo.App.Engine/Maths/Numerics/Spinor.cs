using System;
using System.Numerics;

namespace Warp.NET.Maths.Numerics;

public readonly struct Spinor : IEquatable<Spinor>
{
	private readonly float _real;
	private readonly float _complex;

	public Spinor(float real, float complex)
	{
		_real = real;
		_complex = complex;
	}

	public Spinor(float angle)
	{
		_real = MathF.Cos(angle / 2);
		_complex = MathF.Sin(angle / 2);
	}

	public Spinor(Vector2 directionalVector)
		: this(MathF.Atan2(directionalVector.Y, directionalVector.X))
	{
	}

	public static Spinor operator +(Spinor left, Spinor right)
		=> new(left._real + right._real, left._complex + right._complex);

	public static Spinor operator *(Spinor left, Spinor right)
		=> new(left._real * right._real - left._complex * right._complex, left._real * right._complex + left._complex * right._real);

	public static bool operator ==(Spinor left, Spinor right)
		=> left.Equals(right);

	public static bool operator !=(Spinor left, Spinor right)
		=> !(left == right);

	public Spinor GetScale(float scale)
		=> new(_real * scale, _complex * scale);

	public Spinor GetInvert()
	{
		Spinor s = new(_real, -_complex);
		return s.GetScale(s.GetLengthSquared());
	}

	public float GetLength()
		=> MathF.Sqrt(_real * _real + _complex * _complex);

	public float GetLengthSquared()
		=> _real * _real + _complex * _complex;

	public Spinor GetNormalized()
	{
		float length = GetLength();
		return new(_real / length, _complex / length);
	}

	public float GetAngle()
		=> MathF.Atan2(_complex, _real) * 2;

	public Vector2 GetDirectionalVector()
	{
		float angle = GetAngle();
		return new(MathF.Cos(angle), MathF.Sin(angle));
	}

	public static Spinor Lerp(Spinor from, Spinor to, float amount)
		=> (from.GetScale(1 - amount) + to.GetScale(amount)).GetNormalized();

	public static Spinor Slerp(Spinor from, Spinor to, float amount)
	{
		float toReal, toComplex, scale0, scale1;

		float cosOmega = from._real * to._real + from._complex * to._complex;
		if (cosOmega < 0)
		{
			cosOmega = -cosOmega;
			toComplex = -to._complex;
			toReal = -to._real;
		}
		else
		{
			toComplex = to._complex;
			toReal = to._real;
		}

		if (1 - cosOmega > 0.001)
		{
			float omega = MathF.Acos(cosOmega);
			float sinOmega = MathF.Sin(omega);
			scale0 = MathF.Sin((1 - amount) * omega) / sinOmega;
			scale1 = MathF.Sin(amount * omega) / sinOmega;
		}
		else
		{
			scale0 = 1 - amount;
			scale1 = amount;
		}

		return new(scale0 * from._real + scale1 * toReal, scale0 * from._complex + scale1 * toComplex);
	}

	public static float Slerp(float fromAngle, float toAngle, float amount)
	{
		Spinor from = new(MathF.Cos(fromAngle / 2), MathF.Sin(fromAngle / 2));
		Spinor to = new(MathF.Cos(toAngle / 2), MathF.Sin(toAngle / 2));
		return Slerp(from, to, amount).GetAngle();
	}

	public static Spinor Add(Spinor left, Spinor right)
		=> left + right;

	public static Spinor Multiply(Spinor left, Spinor right)
		=> left * right;

	public override bool Equals(object? obj)
	{
		if (obj is not Spinor other)
			return false;

		return Equals(other);
	}

	public bool Equals(Spinor other)
		=> _real == other._real && _complex == other._complex;

	public override int GetHashCode()
		=> HashCode.Combine(_real, _complex);
}
