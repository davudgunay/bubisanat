using bubisanat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace bubisanat.Controllers
{
    public class UsersController : Controller
    {
        SignInManager<ApplicationUser> _signInManager;

        public UsersController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View(_signInManager.UserManager.Users);
        }
        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserName,Email,Password,ConfirmPassword,Agreed")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                if (applicationUser.Agreed == true)
                {
                    _signInManager.UserManager.CreateAsync(applicationUser, applicationUser.Password).Wait();
                    return Redirect("~/");
                }
            }
            return View(applicationUser);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string userName, string password)
        {
            Microsoft.AspNetCore.Identity.SignInResult identityResult;

                identityResult = _signInManager.PasswordSignInAsync(userName, password, false, false).Result;

                if (identityResult.Succeeded == true)
                {
                    return Redirect("~/");
                }
            
            ApplicationUser applicationUser= _signInManager.UserManager.FindByEmailAsync(userName).Result;
            identityResult = _signInManager.PasswordSignInAsync(applicationUser.UserName, password, false, false).Result;

            if (identityResult.Succeeded == true)
            {
                return Redirect("~/");
            }

            return View();
        }
    }
}
