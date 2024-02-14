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
            User? checkUser = (await _context.User.ToListAsync())
                .Where(User => User.Login == login && User.Password == password)
                .ToList()?[0]; //�������� �� count > 0

            if (checkUser != null)
                Program.currentUser = checkUser;


            return View();
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
    }
}
