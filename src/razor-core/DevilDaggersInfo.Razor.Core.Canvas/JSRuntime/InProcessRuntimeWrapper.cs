using Microsoft.JSInterop;

namespace DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

public class InProcessRuntimeWrapper : IJSRuntimeWrapper
{
	private readonly IJSInProcessRuntime _runtime;

	public InProcessRuntimeWrapper(IJSInProcessRuntime runtime)
	{
		_runtime = runtime;
	}

	public void Invoke(string identifier, string elementId) => InvokeReturn<object>(identifier, elementId);
	public void Invoke<T0>(string identifier, string elementId, T0 arg0) => InvokeReturn<T0, object>(identifier, elementId, arg0);
	public void Invoke<T0, T1>(string identifier, string elementId, T0 arg0, T1 arg1) => InvokeReturn<T0, T1, object>(identifier, elementId, arg0, arg1);
	public void Invoke<T0, T1, T2>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2) => InvokeReturn<T0, T1, T2, object>(identifier, elementId, arg0, arg1, arg2);
	public void Invoke<T0, T1, T2, T3>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3) => InvokeReturn<T0, T1, T2, T3, object>(identifier, elementId, arg0, arg1, arg2, arg3);
	public void Invoke<T0, T1, T2, T3, T4>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => InvokeReturn<T0, T1, T2, T3, T4, object>(identifier, elementId, arg0, arg1, arg2, arg3, arg4);
	public void Invoke<T0, T1, T2, T3, T4, T5>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => InvokeReturn<T0, T1, T2, T3, T4, T5, object>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5);
	public void Invoke<T0, T1, T2, T3, T4, T5, T6>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => InvokeReturn<T0, T1, T2, T3, T4, T5, T6, object>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
	public void Invoke<T0, T1, T2, T3, T4, T5, T6, T7>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => InvokeReturn<T0, T1, T2, T3, T4, T5, T6, T7, object>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

	public TResult InvokeReturn<TResult>(string identifier, string elementId) => _runtime.Invoke<TResult>(identifier, elementId);
	public TResult InvokeReturn<T0, TResult>(string identifier, string elementId, T0 arg0) => _runtime.Invoke<TResult>(identifier, elementId, arg0);
	public TResult InvokeReturn<T0, T1, TResult>(string identifier, string elementId, T0 arg0, T1 arg1) => _runtime.Invoke<TResult>(identifier, elementId, arg0, arg1);
	public TResult InvokeReturn<T0, T1, T2, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2) => _runtime.Invoke<TResult>(identifier, elementId, arg0, arg1, arg2);
	public TResult InvokeReturn<T0, T1, T2, T3, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3) => _runtime.Invoke<TResult>(identifier, elementId, arg0, arg1, arg2, arg3);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => _runtime.Invoke<TResult>(identifier, elementId, arg0, arg1, arg2, arg3, arg4);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, T5, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => _runtime.Invoke<TResult>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, T5, T6, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => _runtime.Invoke<TResult>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
	public TResult InvokeReturn<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => _runtime.Invoke<TResult>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
}
