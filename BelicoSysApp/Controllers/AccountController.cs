using BelicoSysApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BelicoSysApp.Controllers
{
    public class AccountController : Controller
    {
        
    // GET: Account/Login
    public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            // Dummy login logic
            if (model.Username == "admin" && model.Password == "admin")
            {
                // Successful login
                // You can implement your own authentication logic here (e.g., setting cookies, session variables, etc.)

                return RedirectToAction("Index", "Home");
            }

            // Failed login
            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        public ActionResult Logout()
        {
            // Implement your logout logic here
            // For example, clearing authentication cookies or session variables

            // Redirect to the login page or any other desired page
            return RedirectToAction("Login", "Account");
        }
    }

}
