namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;

public interface IFileDialogLayout : IExtendedLayout
{
	void SetComponentsFromPath(string path);
}
