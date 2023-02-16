

using JobBoard.Models;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,SignInManager<AppUser> signInManager )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }


        //public async Task<IActionResult> CreateSuperAdmin()
        //{
        //    AppUser SuperAdmin = new AppUser
        //    {
        //        Email = "Raphiyev@gmail.com",
        //        UserName = "SuperAdmin",
        //        FullName = "Azer Raphiyev",
        //        Image = "rəngli.png"
        //    };
        //    var result = await userManager.CreateAsync(SuperAdmin, "Azer2233");
        //    return Ok(SuperAdmin);
        //}
        //public async Task<IActionResult> CreateRole()
        //{
        //    IdentityRole role1=new IdentityRole("SuperAdmin");
        //    IdentityRole role2=new IdentityRole("Admin");
        //    IdentityRole role3=new IdentityRole("Company");
        //    IdentityRole role4=new IdentityRole("Member");
        //    await roleManager.CreateAsync(role1);
        //    await roleManager.CreateAsync(role2);
        //    await roleManager.CreateAsync(role3);
        //    await roleManager.CreateAsync(role4);
        //    return Ok("Created Roles");
        //}
        //public async Task<IActionResult> AddRole()
        //{
        //    AppUser user = await userManager.FindByEmailAsync("Raphiyev@gmail.com");
        //    await userManager.AddToRoleAsync(user, "SuperAdmin");
        //    return Ok("Add Role");
        //}





        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModels loginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser member = await userManager.FindByEmailAsync(loginVM.Email);
            if (member == null)
            {
                ModelState.AddModelError("", "Email or Password invalid");
                return View();
            }
            var result = await signInManager.PasswordSignInAsync(member, loginVM.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password invalid");
                return View();
            }
            return RedirectToAction("Index", "dashboard");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModels registerVM)
        {
            if (!ModelState.IsValid)  return View();
            AppUser user = await userManager.FindByEmailAsync(registerVM.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "Email Is token");
                return View();
            }
            user = await userManager.FindByNameAsync(registerVM.UserName);

            if (user != null)
            {
                ModelState.AddModelError("Username", "Username Is token");
                return View();
            }
            AppUser member = new AppUser
            {
                FullName = $"{registerVM.Name} {registerVM.Surname}",
                Email = registerVM.Email,
                UserName = registerVM.UserName,
                Role="Admin",
                Image= "default-profile.png"
			};
            var result = await userManager.CreateAsync(member,registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    return View();
                }
            }
            await userManager.AddToRoleAsync(member, "Admin");

            return RedirectToAction("index","dashboard");
        }
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
        
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            AppUser appUser= await userManager.FindByEmailAsync(forgotPasswordVM.Email);
            if (appUser == null) { return BadRequest(); }

            string token=await userManager.GeneratePasswordResetTokenAsync(appUser);

            string link = Url.Action("ResetPassword","Account",new {userid=appUser.Id , token=token},HttpContext.Request.Scheme);


            return Ok(link);
        }
        public async Task< IActionResult> ResetPassword(string userid,string token)
        {
			if(string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(token)) { return BadRequest(); }
			AppUser appUser = await userManager.FindByIdAsync(userid);
			if (appUser == null) { return BadRequest(); }
			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM,string userid,string token)
        {
			if (string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(token)) { return BadRequest(); }
			AppUser appUser = await userManager.FindByIdAsync(userid);
			if (appUser == null) { return BadRequest(); }
            token = token + "vvvn";
            var res = await userManager.ResetPasswordAsync(appUser,token,resetPasswordVM.NewPassword);
            if (res.Succeeded) { return RedirectToAction("Login"); }
            return BadRequest();
		}

	}
}
