namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("Roles")]
public class RoleEntity
{
	[Key]
	[StringLength(32)]
	public string Name { get; set; } = null!;

	public List<UserRoleEntity>? UserRoles { get; set; }
}
