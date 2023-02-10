namespace JobBoard.Database
{
	public class JobBoardContext:IdentityDbContext
	{
		public JobBoardContext(DbContextOptions<JobBoardContext> options): base (options)
		{
		}
			public DbSet<AppUser> Users { get; set; }
		public DbSet<Member> members { get; set; }
		public DbSet<Company> companies { get; set; }
		public DbSet<Position> positions { get; set; }
		public DbSet<Team> JonTeamMembers { get; set; }
		public DbSet<MiniInfoBar> miniInfoBars { get; set; }
		public DbSet<Catagory> catagories { get; set; }
		public DbSet<Authour> authours { get; set; }
		public DbSet<Blog> blogs { get; set; }
	}
}
