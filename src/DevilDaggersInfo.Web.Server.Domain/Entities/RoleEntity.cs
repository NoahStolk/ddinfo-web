namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("Roles")]
public class RoleEntity
{
	[Key]
	[StringLength(32)]
	public required string Name { get; set; }

	public List<UserRoleEntity>? UserRoles { get; set; }
}
