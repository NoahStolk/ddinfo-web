namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("UserRoles")]
public sealed class UserRoleEntity
{
	public int UserId { get; set; }

	[ForeignKey(nameof(UserId))]
	public UserEntity? User { get; set; }

	// Matches RoleEntity.Name. EF's convention copies the principal key's length onto this foreign key, but the
	// attribute makes the composite primary key's column width explicit rather than convention-dependent.
	[StringLength(32)]
	public required string RoleName { get; set; }

	[ForeignKey(nameof(RoleName))]
	public RoleEntity? Role { get; set; }
}
