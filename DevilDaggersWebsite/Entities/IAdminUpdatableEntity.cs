namespace DevilDaggersWebsite.Entities
{
	public interface IAdminUpdatableEntity<TAdminDto> : IEntity
		where TAdminDto : class
	{
		public void Create(ApplicationDbContext dbContext, TAdminDto adminDto);
		public void Edit(ApplicationDbContext dbContext, TAdminDto adminDto);
		public void CreateManyToManyRelations(ApplicationDbContext dbContext, TAdminDto adminDto);
		public TAdminDto Populate();
	}
}
