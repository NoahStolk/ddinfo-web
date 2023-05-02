using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace DevilDaggersInfo.App;

public static class GlobalContext
{
	public static InternalResources? InternalResources { get; set; }
	public static GameResources? GameResources { get; set; }
	public static GL? Gl { get; set; }
	public static IInputContext? InputContext { get; set; }
	public static IWindow? Window { get; set; }
}
