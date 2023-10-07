namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("Users")]
public class UserEntity : IAuditable
{
	[Key]
	public int Id { get; set; }

	[StringLength(32)]
	public required string Name { get; set; }

	public required byte[] PasswordHash { get; set; }

	public required byte[] PasswordSalt { get; set; }

	public DateTime? DateRegistered { get; init; }

	public List<UserRoleEntity>? UserRoles { get; set; }

	public int? PlayerId { get; set; }

	[ForeignKey(nameof(PlayerId))]
	public PlayerEntity? Player { get; set; }
}
