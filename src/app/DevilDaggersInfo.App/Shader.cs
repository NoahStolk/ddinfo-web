using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App;

public class Shader : IDisposable
{
	private readonly uint _handle;
	private readonly GL _gl;

	public Shader(GL gl, string vertexCode, string fragmentCode)
	{
		_gl = gl;

		uint vertex = LoadShader(ShaderType.VertexShader, vertexCode);
		uint fragment = LoadShader(ShaderType.FragmentShader, fragmentCode);
		_handle = _gl.CreateProgram();
		_gl.AttachShader(_handle, vertex);
		_gl.AttachShader(_handle, fragment);
		_gl.LinkProgram(_handle);
		_gl.GetProgram(_handle, GLEnum.LinkStatus, out int status);
		if (status == 0)
			throw new Exception($"Program failed to link with error: {_gl.GetProgramInfoLog(_handle)}");

		_gl.DetachShader(_handle, vertex);
		_gl.DetachShader(_handle, fragment);
		_gl.DeleteShader(vertex);
		_gl.DeleteShader(fragment);
	}

	public void Use()
	{
		_gl.UseProgram(_handle);
	}

	private int GetUniformLocation(string name)
	{
		int location = _gl.GetUniformLocation(_handle, name);
		if (location == -1)
			throw new Exception($"{name} uniform not found on shader.");

		return location;
	}

	public void SetUniform(string name, int value)
	{
		_gl.Uniform1(GetUniformLocation(name), value);
	}

	public unsafe void SetUniform(string name, Matrix4x4 value)
	{
		_gl.UniformMatrix4(GetUniformLocation(name), 1, false, (float*)&value);
	}

	public void SetUniform(string name, float value)
	{
		_gl.Uniform1(GetUniformLocation(name), value);
	}

	public void SetUniform(string name, Span<Vector3> value)
	{
		if (value.IsEmpty)
			return;

		Span<float> array = stackalloc float[value.Length * 3];
		for (int i = 0; i < value.Length; i++)
		{
			Vector3 vector = value[i];
			array[i * 3] = vector.X;
			array[i * 3 + 1] = vector.Y;
			array[i * 3 + 2] = vector.Z;
		}

		_gl.Uniform3(GetUniformLocation(name), array);
	}

	public void SetUniform(string name, Span<float> value)
	{
		if (value.IsEmpty)
			return;

		_gl.Uniform1(GetUniformLocation(name), value);
	}

	public void Dispose()
	{
		_gl.DeleteProgram(_handle);
	}

	private uint LoadShader(ShaderType type, string src)
	{
		uint handle = _gl.CreateShader(type);
		_gl.ShaderSource(handle, src);
		_gl.CompileShader(handle);
		string infoLog = _gl.GetShaderInfoLog(handle);
		if (!string.IsNullOrWhiteSpace(infoLog))
			throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");

		return handle;
	}
}
