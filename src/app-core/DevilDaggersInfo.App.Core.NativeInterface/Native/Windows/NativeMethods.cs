using System.Runtime.InteropServices;

namespace DevilDaggersInfo.App.Core.NativeInterface.Native.Windows;

internal static class NativeMethods
{
	[DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
	internal static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
}
