namespace DevilDaggersInfo.Web.Client.StateObjects;

public interface IStateObject<out TModel>
{
	TModel ToModel();
}
