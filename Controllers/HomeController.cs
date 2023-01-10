using bubisanat.Data;
using bubisanat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace bubisanat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Category).Include(p => p.NextPost).Include(p => p.PreviousPost).Include(p => p.Author);
            return View(applicationDbContext.ToList());
        }

        public async Task<IActionResult> Hakkimizda()
        {
            return View();
        }
        public async Task<IActionResult> Yardim()
        {
            return View();
        }
        public async Task<IActionResult> KullaniciSozlesmesi()
        {
            return View();
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}