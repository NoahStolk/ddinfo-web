namespace DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;

public interface IJSRuntimeWrapper
{
	void Invoke(string identifier, string elementId);
	void Invoke<T0>(string identifier, string elementId, T0 arg0);
	void Invoke<T0, T1>(string identifier, string elementId, T0 arg0, T1 arg1);
	void Invoke<T0, T1, T2>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2);
	void Invoke<T0, T1, T2, T3>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3);
	void Invoke<T0, T1, T2, T3, T4>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	void Invoke<T0, T1, T2, T3, T4, T5>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	void Invoke<T0, T1, T2, T3, T4, T5, T6>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	void Invoke<T0, T1, T2, T3, T4, T5, T6, T7>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

	TResult InvokeReturn<TResult>(string identifier, string elementId);
	TResult InvokeReturn<T0, TResult>(string identifier, string elementId, T0 arg0);
	TResult InvokeReturn<T0, T1, TResult>(string identifier, string elementId, T0 arg0, T1 arg1);
	TResult InvokeReturn<T0, T1, T2, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2);
	TResult InvokeReturn<T0, T1, T2, T3, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3);
	TResult InvokeReturn<T0, T1, T2, T3, T4, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	TResult InvokeReturn<T0, T1, T2, T3, T4, T5, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	TResult InvokeReturn<T0, T1, T2, T3, T4, T5, T6, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	TResult InvokeReturn<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(string identifier, string elementId, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
}
