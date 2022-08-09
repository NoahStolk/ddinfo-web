namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("AuditLogEdits")]
public class AuditLogEditEntity
{
	[Key]
	public int Id { get; set; }

	public DateTime DateTime { get; set; }

	[StringLength(64)]
	public string EntityName { get; set; } = null!;

	public int EntityId { get; set; }

	public int ChangedByUserId { get; set; }

	[StringLength(64)]
	public string PropertyName { get; set; } = null!;

	[StringLength(64)]
	public string OriginalValue { get; set; } = null!;

	[StringLength(64)]
	public string CurrentValue { get; set; } = null!;
}
