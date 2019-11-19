using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Core.Entity;
using Application.Web.Services;
using Application.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Web.Controllers
{
    public class AccountController : Controller
    {
        public readonly ILogger<AccountController> _logger;

        public UserManager<User> UserManager { get; }

        public SignInManager<User> SignInManager { get; }

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            ILogger<AccountController> logger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result =
                await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                _logger.LogInformation("logged in {userName}.", model.UserName);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                _logger.LogWarning("failed to log in {userName}.", model.UserName);
                ModelState.AddModelError("", "用户名或密码错误");
                return View(model);
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    CreateOn = DateTime.Now,
                    LastTime = DateTime.Now
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {userName} was created.", model.Email);
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, code = code},
                        HttpContext.Request.Scheme);
                    await MessageService.SendEmailAsync(model.Email, "Confirm your account", "please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    if (string.Equals(user.UserName, "admin", StringComparison.OrdinalIgnoreCase))
                    {
                        await UserManager.AddClaimAsync(user, new Claim("Admin", "Allowed"));
                    }
                    return RedirectToAction("Login");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            var userName = HttpContext.User.Identity.Name;

            await SignInManager.SignOutAsync();

            _logger.LogInformation("{userName} logged out.", userName);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AccessDenied()
        {
            return RedirectToAction("Index", "Home");
        }

        #region 辅助方法

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                _logger.LogWarning("error in create user:{error}", error.Description);
            }
        }

        private Task<User> GetCurrentUserAsync()
        {
            return UserManager.GetUserAsync(HttpContext.User);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}