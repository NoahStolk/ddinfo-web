namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public interface IUserService
{
	UserEntity? Authenticate(string name, string password);

	UserEntity Create(string name, string password);

	void UpdateName(int id, string name);

	void UpdatePassword(int id, string password);

	void Delete(int id);

	string GenerateJwt(UserEntity userEntity);

	UserEntity? GetUserByJwt(string jwt);
}
