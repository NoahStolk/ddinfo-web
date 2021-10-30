namespace DevilDaggersInfo.Web.BlazorWasm.Server.Scoped;

public interface IUserService
{
	UserEntity? Authenticate(string name, string password);

	List<UserEntity> GetAll();

	UserEntity? GetById(int id);

	UserEntity Create(string name, string password);

	void Update(int id, string? name, string? password);

	void Delete(int id);
}
