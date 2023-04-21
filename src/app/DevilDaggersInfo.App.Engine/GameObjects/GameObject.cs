namespace Warp.NET.GameObjects;

public abstract class GameObject : IGameObject
{
	public virtual void Update()
	{
	}

	public virtual void PrepareUpdate()
	{
		PrepareUpdateInterpolation();
	}

	public virtual void PrepareRender()
	{
		PrepareRenderInterpolation();
	}

	public virtual void PrepareUpdateInterpolation()
	{
	}

	public virtual void PrepareRenderInterpolation()
	{
	}

	public virtual void Add()
	{
		WarpBase.Game.Add(this);
		AddChildren();
	}

	public virtual void Remove()
	{
		WarpBase.Game.Remove(this);
		RemoveChildren();
	}

	public virtual void AddChildren()
	{
	}

	public virtual void RemoveChildren()
	{
	}
}
