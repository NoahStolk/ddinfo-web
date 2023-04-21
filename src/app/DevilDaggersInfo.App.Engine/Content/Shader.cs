using Silk.NET.OpenGL;

namespace Warp.NET.Content;

public class Shader
{
	public Shader(string vertexCode, string? geometryCode, string fragmentCode)
	{
		uint vs = Gl.Gl.CreateShader(ShaderType.VertexShader);
		Gl.Gl.ShaderSource(vs, vertexCode);
		Gl.Gl.CompileShader(vs);
		CheckShaderStatus(vs);

		uint? gs = null;
		if (geometryCode != null)
		{
			gs = Gl.Gl.CreateShader(ShaderType.GeometryShader);
			Gl.Gl.ShaderSource(gs.Value, geometryCode);
			Gl.Gl.CompileShader(gs.Value);
			CheckShaderStatus(gs.Value);
		}

		uint fs = Gl.Gl.CreateShader(ShaderType.FragmentShader);
		Gl.Gl.ShaderSource(fs, fragmentCode);
		Gl.Gl.CompileShader(fs);
		CheckShaderStatus(fs);

		Id = Gl.Gl.CreateProgram();

		Gl.Gl.AttachShader(Id, vs);
		if (gs.HasValue)
			Gl.Gl.AttachShader(Id, gs.Value);
		Gl.Gl.AttachShader(Id, fs);
		Gl.Gl.LinkProgram(Id);

		Gl.Gl.DetachShader(Id, vs);
		if (gs.HasValue)
			Gl.Gl.DetachShader(Id, gs.Value);
		Gl.Gl.DetachShader(Id, fs);

		Gl.Gl.DeleteShader(vs);
		if (gs.HasValue)
			Gl.Gl.DeleteShader(gs.Value);
		Gl.Gl.DeleteShader(fs);
	}

	public uint Id { get; }

	private static void CheckShaderStatus(uint shaderId)
	{
		string infoLog = Gl.Gl.GetShaderInfoLog(shaderId);
		if (!string.IsNullOrWhiteSpace(infoLog))
			throw new InvalidOperationException($"Shader compile error: {infoLog}");
	}

	public void Use()
	{
		Gl.Gl.UseProgram(Id);
	}

	/// <summary>
	/// Returns the uniform location for this <paramref name="shaderId"/> and <paramref name="uniformName"/>.
	/// This method is called by generated code which runs on application startup, and does not need to be used anywhere else.
	/// </summary>
	public static int GetUniformLocation(uint shaderId, string uniformName)
	{
		int location = Gl.Gl.GetUniformLocation(shaderId, uniformName);
		if (location == -1)
			throw new InvalidOperationException($"Could not get location for uniform '{uniformName}'.");

		return location;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetBool(int uniformLocation, bool value)
	{
		SetInt(uniformLocation, value ? 1 : 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetInt(int uniformLocation, int value)
	{
		Gl.Gl.Uniform1(uniformLocation, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetFloat(int uniformLocation, float value)
	{
		Gl.Gl.Uniform1(uniformLocation, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetVector2(int uniformLocation, Vector2 value)
	{
		Gl.Gl.Uniform2(uniformLocation, value.X, value.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetVector3(int uniformLocation, Vector3 value)
	{
		Gl.Gl.Uniform3(uniformLocation, value.X, value.Y, value.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetVector4(int uniformLocation, Vector4 value)
	{
		Gl.Gl.Uniform4(uniformLocation, value.X, value.Y, value.Z, value.W);
	}

	public static void SetMatrix4x4(int uniformLocation, Matrix4x4 value)
	{
		Span<float> data = stackalloc float[16]
		{
			value.M11, value.M12, value.M13, value.M14,
			value.M21, value.M22, value.M23, value.M24,
			value.M31, value.M32, value.M33, value.M34,
			value.M41, value.M42, value.M43, value.M44,
		};
		Gl.Gl.UniformMatrix4(uniformLocation, 1, false, data);
	}

	public static void SetFloatArray(int uniformLocation, ReadOnlySpan<float> values)
	{
		if (values.IsEmpty)
			return;

		Gl.Gl.Uniform1(uniformLocation, values);
	}

	public static void SetVector3Array(int uniformLocation, ReadOnlySpan<Vector3> values)
	{
		if (values.IsEmpty)
			return;

		Span<float> array = stackalloc float[values.Length * 3];
		for (int i = 0; i < values.Length; i++)
		{
			Vector3 vector = values[i];
			array[i * 3] = vector.X;
			array[i * 3 + 1] = vector.Y;
			array[i * 3 + 2] = vector.Z;
		}

		Gl.Gl.Uniform3(uniformLocation, array);
	}
}
