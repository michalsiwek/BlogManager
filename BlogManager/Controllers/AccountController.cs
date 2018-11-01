﻿using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BlogManager.Models;
using BlogManager.Models.Accounts;
using BlogManager.Helpers.Enums;
using System.Net;
using BlogManager.Helpers;

namespace BlogManager.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;

        public AccountController()
        {
            _context = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _context = new ApplicationDbContext();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.CustomUserSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case CustomSignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case CustomSignInStatus.NeedToBeActivate:
                    ModelState.AddModelError("", "You need to wait for account activation");
                    return View(model);
                case CustomSignInStatus.LockedOut:
                    return View("Lockout");
                case CustomSignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case CustomSignInStatus.Failure:
                    ModelState.AddModelError("", "Incorrect login or password");
                    return View(model);
                default:
                    ModelState.AddModelError("", "Something went wrong. Please try again later.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
                return RedirectToAction("Index", "Home");

            var viewModel = new RegisterViewModel
            {
                Account = new Account(),
                PasswordRecoveryQuestions = _context.PasswordRecoveryQuestions.ToList()
            };
            return View(viewModel);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Account
                {
                    UserName = model.Account.Email,
                    Email = model.Account.Email,
                    Nickname = model.Account.Nickname,
                    FirstName = model.Account.FirstName,
                    LastName = model.Account.LastName,
                    PasswordRecoveryAnswer = model.Account.PasswordRecoveryAnswer,
                    CreateDate = DateTime.Now,
                    IsActive = false
                };
                
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    var dbAccount = _context.Users.SingleOrDefault(a => a.Email.Equals(user.Email));
                    if (dbAccount == null)
                        return HttpNotFound();

                    var dbPasswordRecoveryQuestion = _context.PasswordRecoveryQuestions
                        .SingleOrDefault(p => p.Id == model.Account.PasswordRecoveryQuestion.Id);
                    if (dbPasswordRecoveryQuestion == null)
                        return HttpNotFound();

                    dbAccount.PasswordRecoveryQuestion = dbPasswordRecoveryQuestion;
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == default(int) || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string email)
        {
            var dbAccount = _context.Users
                .Include(a => a.PasswordRecoveryQuestion)
                .SingleOrDefault(a => a.Email.Equals(email));

            if (dbAccount == null)
                return HttpNotFound();

            var model = new ResetPasswordViewModel
            {
                Email = dbAccount.Email,
                RecoveryQuestion = dbAccount.PasswordRecoveryQuestion.Question,
                Code = UserManager.GeneratePasswordResetToken(dbAccount.Id)
            };

            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PassReset(ResetPasswordViewModel model)
        {
            var dbAccount = _context.Users.SingleOrDefault(a => a.Email.Equals(model.Email));
            if (dbAccount == null)
                return HttpNotFound();

            if (!dbAccount.PasswordRecoveryAnswer
                .Replace(" ", "").ToLower()
                .Equals(model.Answer.Replace(" ", "").ToLower()))
            {
                ModelState.AddModelError("", "Invalid answer");
                return View("ResetPassword", model);
            }

            var code = _context.PasswordResetVerificationCodes
                .SingleOrDefault(c => c.Account.Id == dbAccount.Id && c.IsActive && c.ExpirationDate > DateTime.Now);

            if (code == null)
            {
                ModelState.AddModelError("", "Verification code expired");
                return View("ResetPassword", model);
            }

            if(!model.VerificationCode.Equals(code.Code))
            {
                ModelState.AddModelError("", "Invalid verification code");
                return View("ResetPassword", model);
            }

            code.IsActive = false;
            _context.SaveChanges();

            var result = await UserManager.ResetPasswordAsync(dbAccount.Id, model.Code, model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation", "Account");

            AddErrors(result);
            return View("ResetPassword", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ProceedPasswordReset(string email)
        {
            var dbAccount = _context.Users
                .Include(a => a.PasswordRecoveryQuestion)
                .SingleOrDefault(a => a.Email.Equals(email));

            if (dbAccount == null)
            {
                ModelState.AddModelError("", "Email is invalid");
                return View("ForgotPassword");
            }                

            var verificationCode = _context.PasswordResetVerificationCodes
                .Include(c => c.Account)
                .SingleOrDefault(c => c.Account.Id == dbAccount.Id && c.IsActive && c.ExpirationDate > DateTime.Now);

            if (verificationCode == null)
            {
                verificationCode = new PasswordResetVerificationCode
                {
                    Account = dbAccount,
                    Code = new Random().Next(10000, 99999).ToString(),
                    ExpirationDate = DateTime.Now.AddMinutes(15),
                    IsActive = true
                };

                _context.PasswordResetVerificationCodes.Add(verificationCode);
                _context.SaveChanges();

                var sendEmailHandler = new SendEmailHandler();
                sendEmailHandler.SendVerificationCode(email, verificationCode.Code);
            }

            if (ModelState.IsValid)
            {
                var model = new ResetPasswordViewModel
                {
                    Email = dbAccount.Email,
                    RecoveryQuestion = dbAccount.PasswordRecoveryQuestion.Question,
                    Code = UserManager.GeneratePasswordResetToken(dbAccount.Id)
                };

                return View("ResetPassword", model);
            }

            return View("ForgotPassword");
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == default(int))
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new Account { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        [Authorize]
        public ActionResult Manage(int id)
        {
            var dbAccount = _context.Users.SingleOrDefault(u => u.Id == id);

            if(dbAccount == null)
                return HttpNotFound();

            if (dbAccount.Id == 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Forbidden action");

            var viewModel = new ManageAccountViewModel
            {
                Account = dbAccount,
                AccountTypes = _context.Roles.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveChanges(Account account)
        {
            var dbAccount = _context.Users.SingleOrDefault(u => u.Id == account.Id);

            if (dbAccount == null)
                return HttpNotFound();


            dbAccount.IsActive = account.IsActive;
            dbAccount.AccountType = _context.Roles.SingleOrDefault(r => r.Id == account.AccountType.Id);

            UserManager.AddToRole(dbAccount.Id, dbAccount.AccountType.Name);

            _context.SaveChanges();

            return RedirectToAction("Index", "Accounts");
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}