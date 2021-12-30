namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("UserRoles")]
public class UserRoleEntity
{
	public int UserId { get; set; }

	[ForeignKey(nameof(UserId))]
	public UserEntity? User { get; set; }

	public string RoleName { get; set; } = null!;

	[ForeignKey(nameof(RoleName))]
	public RoleEntity? Role { get; set; }
}
