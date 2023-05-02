using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App;

public class Shader : IDisposable
{
	private uint _handle;
	private GL _gl;

	public Shader(GL gl, string vertexCode, string fragmentCode)
	{
		_gl = gl;

		uint vertex = LoadShader(ShaderType.VertexShader, vertexCode);
		uint fragment = LoadShader(ShaderType.FragmentShader, fragmentCode);
		_handle = _gl.CreateProgram();
		_gl.AttachShader(_handle, vertex);
		_gl.AttachShader(_handle, fragment);
		_gl.LinkProgram(_handle);
		_gl.GetProgram(_handle, GLEnum.LinkStatus, out var status);
		if (status == 0)
		{
			throw new Exception($"Program failed to link with error: {_gl.GetProgramInfoLog(_handle)}");
		}
		_gl.DetachShader(_handle, vertex);
		_gl.DetachShader(_handle, fragment);
		_gl.DeleteShader(vertex);
		_gl.DeleteShader(fragment);
	}

	public void Use()
	{
		_gl.UseProgram(_handle);
	}

	public void SetUniform(string name, int value)
	{
		int location = _gl.GetUniformLocation(_handle, name);
		if (location == -1)
		{
			throw new Exception($"{name} uniform not found on shader.");
		}
		_gl.Uniform1(location, value);
	}

	public unsafe void SetUniform(string name, Matrix4x4 value)
	{
		//A new overload has been created for setting a uniform so we can use the transform in our shader.
		int location = _gl.GetUniformLocation(_handle, name);
		if (location == -1)
		{
			throw new Exception($"{name} uniform not found on shader.");
		}
		_gl.UniformMatrix4(location, 1, false, (float*)&value);
	}

	public void SetUniform(string name, float value)
	{
		int location = _gl.GetUniformLocation(_handle, name);
		if (location == -1)
		{
			throw new Exception($"{name} uniform not found on shader.");
		}
		_gl.Uniform1(location, value);
	}

	public void SetUniform(string name, Span<Vector3> value)
	{
		if (value.IsEmpty) return;

		int location = _gl.GetUniformLocation(_handle, name);
		if (location == -1)
		{
			throw new Exception($"{name} uniform not found on shader.");
		}

 		Span<float> array = stackalloc float[value.Length * 3];
				for (int i = 0; i < value.Length; i++)
		{
			Vector3 vector = value[i];
			array[i * 3] = vector.X;
			array[i * 3 + 1] = vector.Y;
			array[i * 3 + 2] = vector.Z;
		}

		_gl.Uniform3(location, array);
	}

	public void SetUniform(string name, Span<float> value)
	{
		if (value.IsEmpty) return;

		int location = _gl.GetUniformLocation(_handle, name);
		if (location == -1)
		{
			throw new Exception($"{name} uniform not found on shader.");
		}
		_gl.Uniform1(location, value);
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
		{
			throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
		}

		return handle;
	}
}
