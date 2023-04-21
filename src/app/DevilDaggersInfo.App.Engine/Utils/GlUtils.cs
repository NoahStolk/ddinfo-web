using Silk.NET.OpenGL;

namespace Warp.NET.Utils;

public static class GlUtils
{
	public static unsafe void BufferData<T>(BufferTargetARB target, T[] data, BufferUsageARB usage)
		where T : unmanaged
	{
		fixed (T* d = data)
			Gl.Gl.BufferData(target, (uint)(data.Length * sizeof(T)), d, usage);
	}

	public static unsafe void BufferData<T>(BufferTargetARB target, Span<T> data, BufferUsageARB usage)
		where T : unmanaged
	{
		fixed (T* d = data)
			Gl.Gl.BufferData(target, (uint)(data.Length * sizeof(T)), d, usage);
	}
}
