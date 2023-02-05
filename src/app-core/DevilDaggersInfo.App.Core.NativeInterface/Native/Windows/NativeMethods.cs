using System.Runtime.InteropServices;

namespace DevilDaggersInfo.App.Core.NativeInterface.Native.Windows;

internal static class NativeMethods
{
	[DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
	internal static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

	[DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
	internal static extern bool GetSaveFileName([In, Out] OpenFileName ofn);

	[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	internal static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
}
