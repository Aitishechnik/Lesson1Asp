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

        public IActionResult Registration()
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
                .ToList(); //�������� �� count > 0
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

        //1. �������� ��� form � html
        //2. ��������� ����������� � ����� Where
        //3. ������� ����� ������, ���� ������������ ����� ������ (����� ViewBag)
        //4. ���� ������������ ��������� �������� ����������� ������ �����.
        //5. ������� logout

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(string firstName, string secondName, string tel, string email)
        {
            if (Program.currentUser != null)
                return Redirect("Login");

            if(_context.User.Where(user => user.Login != firstName + secondName).ToList().Count > 0)

            _context.User.Add(new User());

            await _context.SaveChangesAsync();
            return RedirectToAction("Registration");
        }

        
    }
}
