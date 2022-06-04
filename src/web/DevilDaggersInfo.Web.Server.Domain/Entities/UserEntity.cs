namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("Users")]
public class UserEntity
{
	[Key]
	public int Id { get; set; }

	[StringLength(32)]
	public string Name { get; set; } = null!;

	public byte[] PasswordHash { get; set; } = null!;

	public byte[] PasswordSalt { get; set; } = null!;

	public DateTime? DateRegistered { get; init; }

	public List<UserRoleEntity>? UserRoles { get; set; }

	public int? PlayerId { get; set; }

	[ForeignKey(nameof(PlayerId))]
	public PlayerEntity? Player { get; set; }
}
