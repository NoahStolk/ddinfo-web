using DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

namespace DevilDaggersInfo.Razor.Core.Canvas;

public abstract class Canvas
{
	private readonly IJSRuntimeWrapper _wrapper;
	private readonly string _id;

	protected Canvas(IJSRuntimeWrapper wrapper, string id)
	{
		_wrapper = wrapper;
		_id = id;
	}

	public void Invoke(string identifier) => _wrapper.Invoke(identifier, _id);
	public void Invoke<T0>(string identifier, T0 arg0) => _wrapper.Invoke(identifier, _id, arg0);
	public void Invoke<T0, T1>(string identifier, T0 arg0, T1 arg1) => _wrapper.Invoke(identifier, _id, arg0, arg1);
	public void Invoke<T0, T1, T2>(string identifier, T0 arg0, T1 arg1, T2 arg2) => _wrapper.Invoke(identifier, _id, arg0, arg1, arg2);
	public void Invoke<T0, T1, T2, T3>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3) => _wrapper.Invoke(identifier, _id, arg0, arg1, arg2, arg3);
	public void Invoke<T0, T1, T2, T3, T4>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => _wrapper.Invoke(identifier, _id, arg0, arg1, arg2, arg3, arg4);
	public void Invoke<T0, T1, T2, T3, T4, T5>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => _wrapper.Invoke(identifier, _id, arg0, arg1, arg2, arg3, arg4, arg5);
	public void Invoke<T0, T1, T2, T3, T4, T5, T6>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => _wrapper.Invoke(identifier, _id, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
	public void Invoke<T0, T1, T2, T3, T4, T5, T6, T7>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => _wrapper.Invoke(identifier, _id, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

	public TResult InvokeReturn<TResult>(string identifier) => _wrapper.InvokeReturn<TResult>(identifier, _id);
	public TResult InvokeReturn<T0, TResult>(string identifier, T0 arg0) => _wrapper.InvokeReturn<T0, TResult>(identifier, _id, arg0);
	public TResult InvokeReturn<T0, T1, TResult>(string identifier, T0 arg0, T1 arg1) => _wrapper.InvokeReturn<T0, T1, TResult>(identifier, _id, arg0, arg1);
	public TResult InvokeReturn<T0, T1, T2, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2) => _wrapper.InvokeReturn<T0, T1, T2, TResult>(identifier, _id, arg0, arg1, arg2);
	public TResult InvokeReturn<T0, T1, T2, T3, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3) => _wrapper.InvokeReturn<T0, T1, T2, T3, TResult>(identifier, _id, arg0, arg1, arg2, arg3);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => _wrapper.InvokeReturn<T0, T1, T2, T3, T4, TResult>(identifier, _id, arg0, arg1, arg2, arg3, arg4);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, T5, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => _wrapper.InvokeReturn<T0, T1, T2, T3, T4, T5, TResult>(identifier, _id, arg0, arg1, arg2, arg3, arg4, arg5);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, T5, T6, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => _wrapper.InvokeReturn<T0, T1, T2, T3, T4, T5, T6, TResult>(identifier, _id, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => _wrapper.InvokeReturn<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(identifier, _id, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
}
