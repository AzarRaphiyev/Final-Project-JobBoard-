

using JobBoard.Areas.manage.services;
using JobBoard.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<JobBoardContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Def")));
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
	opt.Password.RequiredLength = 8;
	opt.Password.RequireNonAlphanumeric = false;
	opt.Password.RequireDigit = true;
	opt.Password.RequireLowercase = true;
	opt.Password.RequireUppercase = true;
	opt.User.RequireUniqueEmail = false;
}).AddEntityFrameworkStores<JobBoardContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<LayoutServices>();
builder.Services.AddScoped<AdminLayoutService>();
builder.Services.AddTransient<IMailService, MailService>();


var app = builder.Build();



app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
			name: "areas",
			pattern: "{area:exists}/{controller=dashboard}/{action=Index}/{id?}"
		  );



app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
