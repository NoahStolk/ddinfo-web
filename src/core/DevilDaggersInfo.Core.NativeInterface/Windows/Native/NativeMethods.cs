using System.Runtime.InteropServices;

namespace DevilDaggersInfo.Core.NativeInterface.Windows.Native;

internal static class NativeMethods
{
	[DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
	internal static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
}
