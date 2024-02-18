using Lesson1.Data;
using Lesson1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Lesson1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Lesson1Context _context; //DEPENDENCY INJECTION

        public HomeController(ILogger<HomeController> logger, Lesson1Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string login, string password)
        {

            ViewBag.Answer = true;
            var temp = (await _context.User.Include(x => x.UserPermissions).ToListAsync())
                .Where(User => User.Login == login && User.Password == password)
                .ToList(); //проверка на count > 0
            User? checkUser = temp.Count > 0 ? temp[0] : null;
            if (checkUser != null)
            {
                Program.currentUser = checkUser;
                return View();
            }

            ViewBag.Answer = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {

            Program.currentUser = null;

            return RedirectToAction("Index");
        }

        //1. Почитать про form в html
        //2. Докрутить авторизацию в месте Where
        //3. Сделать вывод ошибки, если неправильный логин пароль (через ViewBag)
        //4. Если пользователь залогинен выводить приветствие вместо формы.
        //5. Сделать logout

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
