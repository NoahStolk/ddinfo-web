namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("Roles")]
public class RoleEntity
{
	[Key]
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public List<UserRoleEntity>? UserRoles { get; set; }
}
