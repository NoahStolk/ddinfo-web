namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services.Scoped;

public interface IUserService
{
	UserEntity? Authenticate(string name, string password);

	UserEntity Create(string name, string password);

	void Update(int id, string? name, string? password);

	void Delete(int id);

	string GenerateJwt(UserEntity userEntity);

	UserEntity? GetUserByJwt(string jwt);
}
