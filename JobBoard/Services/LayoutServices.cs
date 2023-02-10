namespace JobBoard.Services
{
	public class LayoutServices
	{
        private readonly UserManager<AppUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LayoutServices(UserManager<AppUser> userManager,IHttpContextAccessor httpContextAccessor)
		{
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<AppUser> GetUser()
        {
            AppUser user = await userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name);
         

            return user;
        }
	}
}
