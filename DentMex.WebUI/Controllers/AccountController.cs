using DentMex.Domain;
using DentMex.Logic;
using DentMex.WebUI.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DentMex.WebUI.Controllers
{
    public class AccountController : AbstractController
    {

        public AccountController(IAccountService AccountService, IVisitService VisitService) : base(AccountService, VisitService) { }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if(HttpContext.User.Identity.IsAuthenticated && !HttpContext.User.IsInRole("Administrator"))
                return RedirectToAction("Index", "Home");

            var model = new RegisterViewModel
            {
                Gender = AccountService.GetGenders().ToList(),
                City = AccountService.GetCities().ToList()
            };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            model.Gender = AccountService.GetGenders().ToList();
            model.City = AccountService.GetCities().ToList();
            if(ModelState.IsValid)
            {
                Account result = new Account
                {
                    AccountLogin = model.AccountLogin,
                    AccountPassword = model.Password,
                    Email = model.Email,
                    InsuranceNumber = model.InsuranceNumber,
                    Pesel = model.Pesel,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PostalCode = model.PostalCode,
                    Phone = model.Phone,
                    CityId = model.CityId,
                    GenderId = model.GenderId,
                    UserRoleId = 4
                };

                bool isRegistered = AccountService.RegisterAccount(result);
                ViewBag.register = isRegistered ? 1 : 2;
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {
            if(ModelState.IsValid)
            {
                var account = AccountService.GetAccountByUsernameAndPassword(model.AccountLogin, model.Password);
                ViewBag.Logged = account;
                ViewBag.isError = ViewBag.Logged == null;
                if(!ViewBag.isError)
                {
                    string formattedLogin = account.FirstName + " " + account.LastName + " (" + account.AccountLogin + ")";
                    FormsAuthentication.SetAuthCookie(formattedLogin, false); //czas zalogowania domyślny - 30 minut

                    if(Url.IsLocalUrl(ReturnUrl))
                        return Redirect(ReturnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            HttpContext.User = new GenericPrincipal(new GenericIdentity(""), null);
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult EditAccount()
        {
            Account account = AccountService.GetAccountByLogin(GetIdFromIdentity(HttpContext.User.Identity.Name));
            var model = new EditAccountViewModel()
            {
                Email = account.Email,
                InsuranceNumber = account.InsuranceNumber,
                Address = account.Address,
                PostalCode = account.PostalCode,
                City = AccountService.GetCities().ToList(),
                CityId = account.CityId,
                Phone = account.Phone,
                PreviousPage = Request.ServerVariables ["HTTP_REFERER"]
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditAccount(EditAccountViewModel model)
        {
            string message = "";
            bool isError = false;  
            model.City = AccountService.GetCities().ToList();

            Account newAccount = new Account();
            Account account = AccountService.GetAccountByLogin(GetIdFromIdentity(HttpContext.User.Identity.Name));
            string oldmail = account.Email;


            if(ModelState.IsValid)
            {
                ModelState.Clear();
                if(model.Email == null || model.Email.Equals(oldmail, StringComparison.OrdinalIgnoreCase))
                {
                    model.Email = account.Email;
                }

                //zmiana hasła
                if(model.Password != null || model.NewPassword != null || model.RepeatPassword != null)
                {
                    if(model.Password == null)
                    {
                        isError = true;
                        message += "Wprowadź stare hasło w polu hasło! <br /> ";
                    }
                    if(model.NewPassword != null && model.Password == null)
                    {
                        isError = true;
                        message += "Aby zmienić hasło, wprowadź stare hasło! <br />";
                    }
                    if(model.Password != null && model.NewPassword == null)
                    {
                        isError = true;
                        message += "Aby zmienić hasło, wprowadź nowe hasło! <br />";
                    }

                    if(model.Password != null && model.NewPassword != null && model.RepeatPassword != null)
                    {
                        if(model.Password.Equals(model.NewPassword, StringComparison.OrdinalIgnoreCase))
                        {
                            isError = true;
                            message += "Aby zmienić hasło, wprowadź nowe hasło! <br />";
                        }

                        if(model.Password.Length < 6)
                        {
                            isError = true;
                            message += "Nowe hasło musi być przynjamniej 6 znakowe oraz musi wystąpić przynajmniej 1-na cyfra! <br />";
                        }
                    }
                    if(!isError)
                    {
                        newAccount.AccountPassword = model.NewPassword;
                    }
                }


                newAccount.AccountId = account.AccountId;
                newAccount.Email = model.Email;
                newAccount.InsuranceNumber = model.InsuranceNumber;
                newAccount.Address = model.Address;
                newAccount.PostalCode = model.PostalCode;
                newAccount.CityId = model.CityId;
                newAccount.Phone = model.Phone;

                if(!isError && AccountService.EditAccount(newAccount))
                {
                    isError = false;
                    message += "Pomyślnie zaktualizowano dane!";

                    string formattedLogin = account.FirstName + " " + account.LastName + " (" + account.AccountId + ")";
                    FormsAuthentication.SetAuthCookie(formattedLogin, false);
                }
                else
                {
                    isError = true;
                    message += "Wystąpił błąd w logice. Spróbuj ponownie później!<br />";
                }
            }

            ViewBag.IsError = isError;
            ViewBag.Message = message;
            return View(model);
        }

        public ActionResult DeleteAccount(string login)
        {
            string id;
            if(HttpContext.User.IsInRole("Administrator") && login != null)
            {
                id = login;
            }
            else
            {
                id = GetIdFromIdentity(HttpContext.User.Identity.Name);
            }

            Account acc = AccountService.GetAccountByLogin(login);
            bool result = AccountService.DeleteAccount(login);
            if(!result)
            {
                TempData ["isError"] = true;
                TempData ["message"] = "Nie udało się usunąć konta " + id + "!";
            }
            else
            {
                TempData ["isError"] = false;
                TempData ["message"] = "Usunięto konto " + id + "!";
            }
            return HttpContext.User.IsInRole("Administrator") ? RedirectToAction("ShowAccounts") : RedirectToAction("Logout");
        }


        [Authorize(Roles = "Administrator")]
        public ActionResult ShowAccounts()
        {
            List<Account> accounts = AccountService.GetAccounts().ToList();
            List<string> cities = new List<string>();
            List<string> genders = new List<string>();
            List<string> roles = new List<string>();

            foreach(var acc in accounts)
            {
                cities.Add(AccountService.GetCity(acc.AccountLogin).CityName);
                genders.Add(AccountService.GetGender(acc.AccountLogin).GenderName);
                roles.Add(AccountService.GetUserRole(acc.AccountLogin).RoleName);
            }

            ShowAccountsViewModel model = new ShowAccountsViewModel()
            {
                Accounts = accounts,
                CityNames = cities,
                GenderNames = genders,
                RoleNames = roles
            };
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult EditPermissions(string login)
        {
            Account acc = AccountService.GetAccountByLogin(login);
            if(acc == null)
            {
                ViewBag.isError = true;
                ViewBag.Message = "Nie ma takiego konta !";
                return View();
            }
            var model = new EditPermissionsViewModel()
            {
                AccountLogin = login,
                UserRoleList = AccountService.GetUserRoles().ToList(),
                UserRoleId = acc.UserRoleId ?? 1,
                PreviousPage = Request.ServerVariables ["HTTP_REFERER"]
            };

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult EditPermissions(EditPermissionsViewModel model)
        {
            bool isEdited = false;
            string message = "";
            model.UserRoleList = AccountService.GetUserRoles().ToList();
            if(ModelState.IsValid)
            {
                isEdited = AccountService.ChangeUserRole(model.AccountLogin, model.UserRoleId);
                message = !isEdited ? "Wystąpił błąd . Spróbuj ponownie" : "Zmiany zostały wprowadzone.";
            }

            ViewBag.isError = !isEdited;
            ViewBag.Message = message;
            return View(model);
        }

	}
}