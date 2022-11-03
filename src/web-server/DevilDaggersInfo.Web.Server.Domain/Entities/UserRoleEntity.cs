namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("UserRoles")]
public class UserRoleEntity
{
	public int UserId { get; set; }

	[ForeignKey(nameof(UserId))]
	public UserEntity? User { get; set; }

	public required string RoleName { get; set; }

	[ForeignKey(nameof(RoleName))]
	public RoleEntity? Role { get; set; }
}
