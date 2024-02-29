using Lesson1.Data;
using Lesson1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            ViewBag.IsLogin = false;
            ViewBag.IsEmail = false;
            ViewBag.IsTel = false;
            return View();
        }

        public IActionResult Account()
        {
            ViewBag.IsLogin = false;
            ViewBag.IsEmail = false;
            ViewBag.IsTel = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string login, string password)
        {

            var temp = (await _context.User.Include(x => x.UserPermissions).ToListAsync())
                .Where(User => User.Login == login && User.Password == password)
                .ToList(); //проверка на count > 0
            User? checkUser = temp.Count > 0 ? temp[0] : null;
            if (checkUser != null)
            {
                Program.currentUser = checkUser;
                return View();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            Program.currentUser = null;

            return Redirect("Index");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(string login, string password, string firstName, string lastName, string tel, string email)
        {
            ViewBag.IsLogin = false;
            ViewBag.IsEmail = false;
            ViewBag.IsTel = false;
            if (Program.currentUser.Role != UserRole.Client)
                return Redirect("Index");
            List<User> temp = _context.User.Where(user => user.Login == login || user.Email == email || user.Tel == tel).ToList();
            if (_context.User.Where(user => user.Login == login || user.Email == email || user.Tel == tel).ToList().Count > 0)
            {
                if (temp[0].Login == login)
                {
                    ViewBag.IsLogin = true;
                }
                if (temp[0].Email == email)
                {
                    ViewBag.IsEmail = true;
                }
                if (temp[0].Tel == tel)
                {
                    ViewBag.IsTel= true;
                }

                return View("Registration");
            }
            else
            {
                _context.User.Add(new User(login, password, firstName, lastName, tel, email));
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Account(string password, string firstName, string lastName, string tel, string email)
        {
            ViewBag.IsLogin = false;
            ViewBag.IsEmail = false;
            ViewBag.IsTel = false;
            
            List<User> temp = _context.User.Where(user => user.Email == email || user.Tel == tel).ToList();
            if (_context.User.Where(user => (user.Email == email && Program.currentUser.Email != email) || 
            (user.Tel == tel && Program.currentUser.Tel != tel)).ToList().Count > 0)
            {
                if (temp[0].Email == email)
                {
                    ViewBag.IsEmail = true;
                }
                if (temp[0].Tel == tel)
                {
                    ViewBag.IsTel = true;
                }
              
                return View("Account");
            }
            else
            {
                //_context.User.Add(new User(login, password, firstName, lastName, tel, email));
                if(Program.currentUser.FirstName != null && 
                    Program.currentUser.FirstName != firstName &&
                    !firstName.IsNullOrEmpty())
                    Program.currentUser.FirstName = firstName;

                if (Program.currentUser.LastName != null &&
                    Program.currentUser.LastName != lastName &&
                    !lastName.IsNullOrEmpty())
                    Program.currentUser.LastName = lastName;

                if (Program.currentUser.Tel != null &&
                    Program.currentUser.Tel != tel &&
                    !tel.IsNullOrEmpty())
                    Program.currentUser.Tel = tel;

                if (Program.currentUser.Email != null &&
                    Program.currentUser.Email != email &&
                    !email.IsNullOrEmpty())
                    Program.currentUser.Email = email;

                if (Program.currentUser.Password != null &&
                    !password.IsNullOrEmpty())
                    Program.currentUser.Password = password;
                await _context.SaveChangesAsync();
            }

            return View("Account");
        }
    }
}
