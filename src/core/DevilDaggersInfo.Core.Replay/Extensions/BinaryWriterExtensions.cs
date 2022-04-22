namespace DevilDaggersInfo.Core.Replay.Extensions;

public static class BinaryWriterExtensions
{
	public static void Write(this BinaryWriter bw, Int16Vec3 vec3)
	{
		bw.Write(vec3.X);
		bw.Write(vec3.Y);
		bw.Write(vec3.Z);
	}

	public static void Write(this BinaryWriter bw, Vector3 vec3)
	{
		bw.Write(vec3.X);
		bw.Write(vec3.Y);
		bw.Write(vec3.Z);
	}

	public static void Write(this BinaryWriter bw, Int16Mat3x3 mat3x3)
	{
		bw.Write(mat3x3.M11);
		bw.Write(mat3x3.M12);
		bw.Write(mat3x3.M13);
		bw.Write(mat3x3.M21);
		bw.Write(mat3x3.M22);
		bw.Write(mat3x3.M23);
		bw.Write(mat3x3.M31);
		bw.Write(mat3x3.M32);
		bw.Write(mat3x3.M33);
	}

	public static void Write(this BinaryWriter bw, Matrix3x3 mat3x3)
	{
		bw.Write(mat3x3.M11);
		bw.Write(mat3x3.M12);
		bw.Write(mat3x3.M13);
		bw.Write(mat3x3.M21);
		bw.Write(mat3x3.M22);
		bw.Write(mat3x3.M23);
		bw.Write(mat3x3.M31);
		bw.Write(mat3x3.M32);
		bw.Write(mat3x3.M33);
	}
}
