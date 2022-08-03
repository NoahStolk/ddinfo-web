using DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

namespace DevilDaggersInfo.Razor.Core.Canvas;

public abstract class WebViewCanvas : Canvas
{
	private readonly WebViewRuntimeWrapper _runtimeWrapper;

	protected WebViewCanvas(string id, WebViewRuntimeWrapper wrapper)
		: base(id)
	{
		_runtimeWrapper = wrapper;
	}

	public async Task InvokeAsync(string identifier) => await _runtimeWrapper.InvokeAsync(identifier, Id);
	public async Task InvokeAsync<T0>(string identifier, T0 arg0) => await _runtimeWrapper.InvokeAsync(identifier, Id, arg0);
	public async Task InvokeAsync<T0, T1>(string identifier, T0 arg0, T1 arg1) => await _runtimeWrapper.InvokeAsync(identifier, Id, arg0, arg1);
	public async Task InvokeAsync<T0, T1, T2>(string identifier, T0 arg0, T1 arg1, T2 arg2) => await _runtimeWrapper.InvokeAsync(identifier, Id, arg0, arg1, arg2);
	public async Task InvokeAsync<T0, T1, T2, T3>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3) => await _runtimeWrapper.InvokeAsync(identifier, Id, arg0, arg1, arg2, arg3);
	public async Task InvokeAsync<T0, T1, T2, T3, T4>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => await _runtimeWrapper.InvokeAsync(identifier, Id, arg0, arg1, arg2, arg3, arg4);
	public async Task InvokeAsync<T0, T1, T2, T3, T4, T5>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => await _runtimeWrapper.InvokeAsync(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5);
	public async Task InvokeAsync<T0, T1, T2, T3, T4, T5, T6>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => await _runtimeWrapper.InvokeAsync(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
	public async Task InvokeAsync<T0, T1, T2, T3, T4, T5, T6, T7>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => await _runtimeWrapper.InvokeAsync(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

	public async Task<TResult> InvokeReturnAsync<TResult>(string identifier) => await _runtimeWrapper.InvokeReturnAsync<TResult>(identifier, Id);
	public async Task<TResult> InvokeReturnAsync<T0, TResult>(string identifier, T0 arg0) => await _runtimeWrapper.InvokeReturnAsync<T0, TResult>(identifier, Id, arg0);
	public async Task<TResult> InvokeReturnAsync<T0, T1, TResult>(string identifier, T0 arg0, T1 arg1) => await _runtimeWrapper.InvokeReturnAsync<T0, T1, TResult>(identifier, Id, arg0, arg1);
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2) => await _runtimeWrapper.InvokeReturnAsync<T0, T1, T2, TResult>(identifier, Id, arg0, arg1, arg2);
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, T3, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3) => await _runtimeWrapper.InvokeReturnAsync<T0, T1, T2, T3, TResult>(identifier, Id, arg0, arg1, arg2, arg3);
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, T3, T4, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => await _runtimeWrapper.InvokeReturnAsync<T0, T1, T2, T3, T4, TResult>(identifier, Id, arg0, arg1, arg2, arg3, arg4); 
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, T3, T4, T5, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => await _runtimeWrapper.InvokeReturnAsync<T0, T1, T2, T3, T4, T5, TResult>(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5);
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, T3, T4, T5, T6, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => await _runtimeWrapper.InvokeReturnAsync<T0, T1, T2, T3, T4, T5, T6, TResult>(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
	public async Task<TResult> InvokeReturnAsync<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => await _runtimeWrapper.InvokeReturnAsync<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(identifier, Id, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
}
