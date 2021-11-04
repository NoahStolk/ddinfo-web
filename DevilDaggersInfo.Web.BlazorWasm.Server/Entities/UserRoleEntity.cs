namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("UserRoles")]
public class UserRoleEntity
{
	public int UserId { get; set; }

	[ForeignKey(nameof(UserId))]
	public UserEntity? User { get; set; }

	public int RoleId { get; set; }

	[ForeignKey(nameof(RoleId))]
	public RoleEntity? Role { get; set; }
}
