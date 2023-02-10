namespace JobBoard.Areas.manage.services
{
    public class AdminLayoutService
    {
            private readonly UserManager<AppUser> userManager;
            private readonly IHttpContextAccessor httpContextAccessor;

            public AdminLayoutService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
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
