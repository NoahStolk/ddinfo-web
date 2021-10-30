namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("Users")]
public class UserEntity
{
	[Key]
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public byte[] PasswordHash { get; set; } = null!;

	public byte[] PasswordSalt { get; set; } = null!;

	public List<UserRoleEntity>? UserRoles { get; set; }
}
