using DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

namespace DevilDaggersInfo.Razor.Core.Canvas;

public abstract class WebAssemblyCanvas : Canvas
{
	private readonly WebAssemblyRuntimeWrapper _runtimeWrapper;

	protected WebAssemblyCanvas(string id, WebAssemblyRuntimeWrapper runtimeWrapper)
		: base(id)
	{
		this._runtimeWrapper = runtimeWrapper;
	}

	public void Invoke(string identifier) => _runtimeWrapper.Invoke(identifier, Id);
	public void Invoke<T0>(string identifier, T0 arg0) => _runtimeWrapper.Invoke(identifier, Id, arg0);
	public void Invoke<T0, T1>(string identifier, T0 arg0, T1 arg1) => _runtimeWrapper.Invoke(identifier, Id, arg0, arg1);
	public void Invoke<T0, T1, T2>(string identifier, T0 arg0, T1 arg1, T2 arg2) => _runtimeWrapper.Invoke(identifier, Id, arg0, arg1, arg2);
	public void Invoke<T0, T1, T2, T3>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3) => _runtimeWrapper.Invoke(identifier, Id, arg0, arg1, arg2, arg3);
	public void Invoke<T0, T1, T2, T3, T4>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => _runtimeWrapper.Invoke(identifier, Id, arg0, arg1, arg2, arg3, arg4);
	public void Invoke<T0, T1, T2, T3, T4, T5>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => _runtimeWrapper.Invoke(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5);
	public void Invoke<T0, T1, T2, T3, T4, T5, T6>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => _runtimeWrapper.Invoke(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
	public void Invoke<T0, T1, T2, T3, T4, T5, T6, T7>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => _runtimeWrapper.Invoke(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

	public TResult InvokeReturn<TResult>(string identifier) => _runtimeWrapper.InvokeReturn<TResult>(identifier, Id);
	public TResult InvokeReturn<T0, TResult>(string identifier, T0 arg0) => _runtimeWrapper.InvokeReturn<T0, TResult>(identifier, Id, arg0);
	public TResult InvokeReturn<T0, T1, TResult>(string identifier, T0 arg0, T1 arg1) => _runtimeWrapper.InvokeReturn<T0, T1, TResult>(identifier, Id, arg0, arg1);
	public TResult InvokeReturn<T0, T1, T2, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2) => _runtimeWrapper.InvokeReturn<T0, T1, T2, TResult>(identifier, Id, arg0, arg1, arg2);
	public TResult InvokeReturn<T0, T1, T2, T3, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3) => _runtimeWrapper.InvokeReturn<T0, T1, T2, T3, TResult>(identifier, Id, arg0, arg1, arg2, arg3);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => _runtimeWrapper.InvokeReturn<T0, T1, T2, T3, T4, TResult>(identifier, Id, arg0, arg1, arg2, arg3, arg4);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, T5, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => _runtimeWrapper.InvokeReturn<T0, T1, T2, T3, T4, T5, TResult>(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, T5, T6, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => _runtimeWrapper.InvokeReturn<T0, T1, T2, T3, T4, T5, T6, TResult>(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => _runtimeWrapper.InvokeReturn<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
}
