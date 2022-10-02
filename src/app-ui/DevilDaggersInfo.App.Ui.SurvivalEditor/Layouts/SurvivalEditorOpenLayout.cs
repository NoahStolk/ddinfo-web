using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Spawnset;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorOpenLayout : Layout, IFileDialogLayout
{
	private readonly TextInput _pathTextInput;
	private readonly PathsWrapper _pathsWrapper;

	public SurvivalEditorOpenLayout()
		: base(Constants.Full)
	{
		Button backButton = new(Rectangle.At(0, 0, 24, 24), LayoutManager.ToSurvivalEditorMainLayout, Color.Black, Color.White, Color.White, Color.Red, "X", TextAlign.Left, 2, FontSize.F12X12);
		_pathTextInput = ComponentBuilder.CreateTextInput(Rectangle.At(0, 24, 1024, 16), false, null, null, null);
		_pathsWrapper = new(Rectangle.At(0, 96, 1024, 640), this);

		NestingContext.Add(backButton);
		NestingContext.Add(_pathTextInput);
		NestingContext.Add(_pathsWrapper);
	}

	public void Update()
	{
	}

	public void Render3d()
	{
	}

	public void Render()
	{
	}

	public void SetComponentsFromPath(string path)
	{
		_pathTextInput.SetText(path);
		_pathsWrapper.Path = path;
		_pathsWrapper.InitializeContent();
	}

	private static void OpenSpawnset(string filePath)
	{
		byte[] bytes = File.ReadAllBytes(filePath);
		if (SpawnsetBinary.TryParse(bytes, out SpawnsetBinary? spawnsetBinary))
		{
			StateManager.SetSpawnset(Path.GetFileName(filePath), spawnsetBinary);
			LayoutManager.ToSurvivalEditorMainLayout();
		}
		else
		{
			// TODO: Show popup.
		}
	}

	// TODO: Refactor.
	private sealed class PathsWrapper : AbstractScrollViewer<PathsWrapper, Paths>
	{
		public PathsWrapper(Rectangle metric, IFileDialogLayout layout)
			: base(metric)
		{
			Rectangle spawnsMetric = Rectangle.At(0, 0, metric.Size.X - 16, metric.Size.Y);

			Content = new(spawnsMetric, this, layout);
			Scrollbar = new(spawnsMetric with { X1 = spawnsMetric.X2, X2 = spawnsMetric.X2 + 16 }, ScrollbarOnChange);

			NestingContext.Add(Content);
			NestingContext.Add(Scrollbar);
		}

		protected override Scrollbar Scrollbar { get; }
		protected override Paths Content { get; }

		public string Path { get; set; } = string.Empty;

		public override void InitializeContent()
		{
			Content.SetComponentsFromPath(Path);

			base.InitializeContent();
		}
	}

	// TODO: Refactor.
	private sealed class Paths : ScrollContent<Paths, PathsWrapper>
	{
		private const int _entryHeight = 16;

		private readonly IFileDialogLayout _layout;
		private readonly List<Button> _subDirectoryButtons = new();

		public Paths(Rectangle metric, PathsWrapper parent, IFileDialogLayout layout)
			: base(metric, parent)
		{
			_layout = layout;
		}

		public override int ContentHeightInPixels => _subDirectoryButtons.Count * _entryHeight;

		public void SetComponentsFromPath(string path)
		{
			foreach (Button button in _subDirectoryButtons)
				NestingContext.Remove(button);

			_subDirectoryButtons.Clear();

			NestingContext.ScrollOffset = default;

			int i = -1;
			DirectoryInfo? parent = Directory.GetParent(path);
			if (parent != null)
				_subDirectoryButtons.Add(new Button.PathButton(new(0, ++i * _entryHeight, 1008, i * _entryHeight + _entryHeight), () => _layout.SetComponentsFromPath(parent.FullName), "..", Color.Green));

			foreach (string directory in Directory.GetDirectories(path))
				_subDirectoryButtons.Add(new Button.PathButton(new(0, ++i * _entryHeight, 1008, i * _entryHeight + _entryHeight), () => _layout.SetComponentsFromPath(directory), Path.GetFileName(directory), Color.Yellow));

			foreach (string file in Directory.GetFiles(path))
				_subDirectoryButtons.Add(new Button.PathButton(new(0, ++i * _entryHeight, 1008, i * _entryHeight + _entryHeight), () => OpenSpawnset(file), Path.GetFileName(file), Color.White));

			foreach (Button button in _subDirectoryButtons)
				NestingContext.Add(button);
		}
	}
}
