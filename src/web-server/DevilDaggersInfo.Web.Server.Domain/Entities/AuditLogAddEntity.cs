namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("AuditLogAdds")]
public class AuditLogAddEntity
{
	[Key]
	public int Id { get; set; }

	public DateTime DateTime { get; set; }

	[StringLength(64)]
	public string EntityName { get; set; } = null!;

	public int EntityId { get; set; }

	public int AddedByUserId { get; set; }
}
