using Microsoft.JSInterop;

namespace DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

public class WebViewRuntimeWrapper
{
	private readonly IJSRuntime _runtime;

	public WebViewRuntimeWrapper(IJSRuntime runtime)
	{
		_runtime = runtime;
	}

	public async Task InvokeAsync(string identifier, string elementId) => await InvokeReturnAsync<object>(identifier, elementId);
	public async Task InvokeAsync<T0>(string identifier, string elementId, T0 arg0) => await InvokeReturnAsync<T0, object>(identifier, elementId, arg0);
	public async Task InvokeAsync<T0, T1>(string identifier, string elementId, T0 arg0, T1 arg1) => await InvokeReturnAsync<T0, T1, object>(identifier, elementId, arg0, arg1);
	public async Task InvokeAsync<T0, T1, T2>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2) => await InvokeReturnAsync<T0, T1, T2, object>(identifier, elementId, arg0, arg1, arg2);
	public async Task InvokeAsync<T0, T1, T2, T3>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3) => await InvokeReturnAsync<T0, T1, T2, T3, object>(identifier, elementId, arg0, arg1, arg2, arg3);
	public async Task InvokeAsync<T0, T1, T2, T3, T4>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => await InvokeReturnAsync<T0, T1, T2, T3, T4, object>(identifier, elementId, arg0, arg1, arg2, arg3, arg4);
	public async Task InvokeAsync<T0, T1, T2, T3, T4, T5>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => await InvokeReturnAsync<T0, T1, T2, T3, T4, T5, object>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5);
	public async Task InvokeAsync<T0, T1, T2, T3, T4, T5, T6>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => await InvokeReturnAsync<T0, T1, T2, T3, T4, T5, T6, object>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
	public async Task InvokeAsync<T0, T1, T2, T3, T4, T5, T6, T7>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => await InvokeReturnAsync<T0, T1, T2, T3, T4, T5, T6, T7, object>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

	public async Task<TResult> InvokeReturnAsync<TResult>(string identifier, string elementId) => await _runtime.InvokeAsync<TResult>(identifier, elementId);
	public async Task<TResult> InvokeReturnAsync<T0, TResult>(string identifier, string elementId, T0 arg0) => await _runtime.InvokeAsync<TResult>(identifier, elementId, arg0);
	public async Task<TResult> InvokeReturnAsync<T0, T1, TResult>(string identifier, string elementId, T0 arg0, T1 arg1) => await _runtime.InvokeAsync<TResult>(identifier, elementId, arg0, arg1);
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2) => await _runtime.InvokeAsync<TResult>(identifier, elementId, arg0, arg1, arg2);
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, T3, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3) => await _runtime.InvokeAsync<TResult>(identifier, elementId, arg0, arg1, arg2, arg3);
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, T3, T4, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => await _runtime.InvokeAsync<TResult>(identifier, elementId, arg0, arg1, arg2, arg3, arg4);
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, T3, T4, T5, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => await _runtime.InvokeAsync<TResult>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5);
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, T3, T4, T5, T6, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => await _runtime.InvokeAsync<TResult>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => await _runtime.InvokeAsync<TResult>(identifier, elementId, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
}
