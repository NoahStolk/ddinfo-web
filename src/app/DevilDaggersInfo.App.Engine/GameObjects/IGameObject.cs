namespace Warp.NET.GameObjects;

public interface IGameObject
{
	void Update();

	void PrepareUpdate();

	void PrepareRender();

	void PrepareUpdateInterpolation();

	void PrepareRenderInterpolation();

	void Add();

	void Remove();

	void AddChildren();

	void RemoveChildren();
}
