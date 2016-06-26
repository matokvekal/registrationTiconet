using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Web;

using System.Web.Caching;
using System.Web.Mvc;

using System.Web.Security;
using Business_Logic;
using Microsoft.AspNet.Identity.EntityFramework;




namespace ticonet
{
    public class AccountController : Controller
    {

        public class ApplicationUser : IdentityUser
        {
            public string mail { get; set; }
            public bool ConfirmedEmail { get; set; }
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View();
            using (LoginLogic login = new LoginLogic())
            {
                if (login.IsExist(model.userName, FormsAuthentication.HashPasswordForStoringInConfigFile(model.password, "sha1")))
                {
                    //if mailconfirm ok
                    if (login.checkIfregisterd(model.userName))
                    {
                        FormsAuthentication.RedirectFromLoginPage(model.userName, model.RememberMe);
                        Session["mailRegistration"] = model.userName;
                        // Login c =login.getLogin(model.userName, FormsAuthentication.HashPasswordForStoringInConfigFile(model.password, "sha1"));

                        int? familyId = LoginLogic.getFamilyId(model.userName, FormsAuthentication.HashPasswordForStoringInConfigFile(model.password, "sha1")).familyId;
                        if (familyId.HasValue)
                        { //redirect to action family}
                            Session["familyId"] = familyId;
                            return RedirectToAction("index", "Family");
                        }
                        else
                            return RedirectToAction("create", "Family");
                    }
                    else
                    {
                        LoginLogic.deleteByEmail(model.userName);
                        ViewBag.message = DictExpressionBuilderSystem.Translate("message.Youdidntconfirmyouremail");
                        //ViewBag.message = "You didnt confirm your email - try to register again and to confirm your email";
                    }

                }
                else
                    ViewBag.message = DictExpressionBuilderSystem.Translate("message.IncorectuserNameorPassword");
                //ViewBag.message = "Incorect userName or Password";
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View();
            //1.check school code if wrong, same page +error -  done
            string SchoolCode;

            if (HttpRuntime.Cache["SchoolCode"] == null)
            {
                using (tblSystemLogic system = new tblSystemLogic())
                {
                    SchoolCode = tblSystemLogic.getSystemValueByKey("SchoolCode").strValue.ToString();
                    HttpRuntime.Cache.Insert("SchoolCode", SchoolCode, null, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(500), CacheItemPriority.High, null);
                }
            }

            SchoolCode = HttpRuntime.Cache["SchoolCode"].ToString();

            if (SchoolCode != model.entranceCode)
            {
                ViewBag.message = DictExpressionBuilderSystem.Translate("message.YoumustentertherightSchoolKod");
                return View();
            }
            if (!Regex.IsMatch(model.Email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                ViewBag.message = DictExpressionBuilderSystem.Translate("message.IncorectE-mail");
                return View();
            }



            //2check email, if not exist same page
            using (LoginLogic login = new LoginLogic())
            {
                Login c = login.getLoginByUser(model.Email);
                if (c != null && c.emailConfirm)
                {

                    //if exist, send email + reset password>>one hour
                    FormsAuthenticationTicket formsTicket = new FormsAuthenticationTicket(
               1,
               model.Email,
               DateTime.Now,
               DateTime.Now.AddMinutes(60 * 5),
               true,
                tblSystemLogic.getSystemValueByKey("schoolName").strValue //for dif with many school in the future

               );
                    string encryptedTicket = FormsAuthentication.Encrypt(formsTicket);


                    //  var user = new ApplicationUser() { UserName = model.userName };
                    //  user.Email = model.userName;
                    //  user.ConfirmedEmail = true;
                    string EmailAdress = tblSystemLogic.getSystemValueByKey("EmailAdress").strValue;
                    string Password = tblSystemLogic.getSystemValueByKey("Password").strValue;
                    string mailServer = tblSystemLogic.getSystemValueByKey("mailServer").strValue;
                    System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                        new System.Net.Mail.MailAddress("harshamaHasaot@gmail.com", "Web Registration"),
                        new System.Net.Mail.MailAddress(model.Email));
                    m.Subject = DictExpressionBuilderSystem.Translate("message.ForgotPassword");
                    //  m.Body = "Please <a href=\"http://www.example.com/login.aspx\">login</a>";
                    m.Body = string.Format("Dear {0}<BR/>" + DictExpressionBuilderSystem.Translate("message.ForgotPasswordMessage") + ": <a href=\"{1}\" title=\"" + DictExpressionBuilderSystem.Translate("message.ForgotPasswordMessage2") + "\">Click here</a>", model.Email, Url.Action("NewPassword", "Account", new { Token = encryptedTicket, Email = model.Email }, Request.Url.Scheme));
                    m.IsBodyHtml = true;
                    //m.Body = string.Format("Dear {0}<BR/>" + DictExpressionBuilderSystem.Translate("message.pleaseclickonthebelow") + ": <a href=\"{1}\" title=\"" + DictExpressionBuilderSystem.Translate("message.UserEmailConfirm") + "\">{1}</a>", model.Email, "rr");
                    //m.Body = string.Format( DictExpressionBuilderSystem.Translate("message.pleaseclickonthebelow") + ": <a href=\"{0}\" title=\"" + DictExpressionBuilderSystem.Translate("message.GetNewPassword") + "\">{1}</a>",  Url.Action("NewPassword", "Account", new { Token = encryptedTicket, Email = model.Email }, Request.Url.Scheme));
                    //m.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(mailServer);
                    smtp.Credentials = new System.Net.NetworkCredential(EmailAdress, Password);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(m);
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                else
                    return View();

            }

            //after reset password replace the password in db
            //go to loginpage



            //var user = await UserManager.FindByNameAsync(model.Email);
            //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            //{
            //    // Don't reveal that the user does not exist or is not confirmed
            //    return View("ForgotPasswordConfirmation");
            //}

            //string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //await UserManager.SendEmailAsync(user.Id, "Reset Password",
            //   "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            //TempData["ViewBagLink"] = callbackUrl;
            //return RedirectToAction("ForgotPasswordConfirmation", "Account");

        }
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {

            return View();
        }


        [AllowAnonymous]
        public ActionResult NewPassword(string Token, string Email)
        {

            if (Token != null)
            {
                using (LoginLogic login = new LoginLogic())
                {
                    FormsAuthenticationTicket formsTicket = FormsAuthentication.Decrypt(Token);
                    string name = formsTicket.Name;
                    string schoolName = tblSystemLogic.getSystemValueByKey("schoolName").strValue;
                    if (!formsTicket.Expired && login.IsExist(Email) && formsTicket.Name == Email && formsTicket.UserData == schoolName)
                    {
                        //  await ResetPassword(Email);
                        Session["Token"] = Token;
                        Session["ResetPasswordEmail"] = Email;
                        return RedirectToAction("ResetPassword");
                    }
                    else
                        return RedirectToAction("Error");
                }
            }
            else
                return RedirectToAction("error");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPassword()
        {
            if (Session["Token"] != null)
            {
                string Token = Session["Token"].ToString();
                //  Session["ResetPasswordEmail"] = Email;
                NewPasswordViewModel model = new NewPasswordViewModel();
                model.Token = Token;
                return View(model);
            }
            else
                return RedirectToAction("error");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(NewPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);





            //reset db pssword
            //delete sessions
            //show succed + link to login
            if (model.Password != model.ConfirmPassword)
                return View(model);
            if (model.Token != Session["Token"].ToString())
                return View(model);


            FormsAuthenticationTicket formsTicket = FormsAuthentication.Decrypt(model.Token);
            string name = formsTicket.Name;
            string schoolName = tblSystemLogic.getSystemValueByKey("schoolName").strValue;
            string Email = Session["ResetPasswordEmail"].ToString();
            using (LoginLogic login = new LoginLogic())
            {
                if (formsTicket.Expired || !login.IsExist(Email) || formsTicket.Name != Email || formsTicket.UserData != schoolName)
                    return View(model);
            }
            if (model.Password.Length < 6)//Add comment to error & check pass is regex ok
                return View(model);
            bool result = false;
            await Task.Run(() =>
            {
                result = ResetPasswordAsync(Email, model.Password);
            });

            if (result)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            return View();
        }

        private bool ResetPasswordAsync(string Email, string Password)
        {
            try
            {
                using (LoginLogic login = new LoginLogic())
                {
                    string password = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "sha1");
                    login.updateLoginPassword(password, Email, Password);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }



        public ActionResult Signout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult unAutorise()
        {
            // HttpRuntime.Cache.Remove("Menu" + SessionHelper.UserTypeName);


            //  SessionHelper.ClearData();
            Session.Clear();
            Session.Abandon();
            //-------------
            HttpContext.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            HttpContext.Response.AddHeader("Pragma", "no-cache");
            HttpContext.Response.AddHeader("Expires", "0");
            FormsAuthentication.SignOut();
            //------
            Session.Clear();
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        //public async Task<ActionResult> Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser() { UserName = model.userName };
        //        user.Email = model.userName;
        //        user.ConfirmedEmail = false;
        //        var result = await UserManager.CreateAsync(user, model.password);
        //        if (result.Succeeded)
        //        {
        //            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
        //                new System.Net.Mail.MailAddress("harshamaHasaot@gmail.com", "Web Registration"),
        //                new System.Net.Mail.MailAddress(user.Email));
        //            m.Subject = "Email confirmation";
        //            m.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
        //            m.IsBodyHtml = true;
        //            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
        //            smtp.Credentials = new System.Net.NetworkCredential("harshamaHasaot@gmail.com", "zaqzaq8*");
        //            smtp.EnableSsl = true;
        //            smtp.Send(m);
        //            return RedirectToAction("Confirm", "Account", new { Email = user.Email });
        //        }
        //        else
        //        {
        //            AddErrors(result);
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            System.Threading.Thread.Sleep(3000);
            ViewBag.Title = DictExpressionBuilderSystem.Translate("message.ConfirmEmailAddressSent");
            ViewBag.message = DictExpressionBuilderSystem.Translate("message.PleasecheckyourEmailInbox");
            ViewBag.Email = Email;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        //public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View();

            string SchoolCode;

            if (HttpRuntime.Cache["SchoolCode"] == null)
            {
                using (tblSystemLogic system = new tblSystemLogic())
                {
                    SchoolCode = tblSystemLogic.getSystemValueByKey("SchoolCode").strValue.ToString();
                    HttpRuntime.Cache.Insert("SchoolCode", SchoolCode, null, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(500), CacheItemPriority.High, null);
                }
            }

            SchoolCode = HttpRuntime.Cache["SchoolCode"].ToString();

            if (SchoolCode != model.entranceCode)
            {
                ViewBag.message = DictExpressionBuilderSystem.Translate("message.YoumustentertherightSchoolKod");
                //ViewBag.message = "You must enter the right School Kod";
                return View();
            }





            using (LoginLogic login = new LoginLogic())
            {
                if (!login.IsExist(model.userName))
                {
                    login.Register(model.userName, FormsAuthentication.HashPasswordForStoringInConfigFile(model.password, "sha1"), model.password);//not secure jest for testing period
                }
                else
                {
                    ViewBag.message = DictExpressionBuilderSystem.Translate("message.Thisusernameisalreadytaken");
                    return View();
                }
            }
            if (!Regex.IsMatch(model.userName, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                ViewBag.message = DictExpressionBuilderSystem.Translate("message.TheusernamemustbeyourE-mail");
                return View();
            }
            FormsAuthenticationTicket formsTicket = new FormsAuthenticationTicket(
                1,
                model.userName,
                DateTime.Now,
                DateTime.Now.AddMinutes(60 * 5),
                true,
                "tichonet"//for dif with many school in the future

                );
            string encryptedTicket = FormsAuthentication.Encrypt(formsTicket);
            var user = new ApplicationUser() { UserName = model.userName };
            user.Email = model.userName;
            user.ConfirmedEmail = true;
            string EmailAdress = tblSystemLogic.getSystemValueByKey("EmailAdress").strValue;
            string Password = tblSystemLogic.getSystemValueByKey("Password").strValue;
            string mailServer = tblSystemLogic.getSystemValueByKey("mailServer").strValue;
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress("harshamaHasaot@gmail.com", "Web Registration"),
                new System.Net.Mail.MailAddress(user.Email));
            m.Subject = DictExpressionBuilderSystem.Translate("message.Emailconfirmation");
            m.Body = string.Format("Dear {0}<BR/>" + DictExpressionBuilderSystem.Translate("message.pleaseclickonthebelow") + ": <a href=\"{1}\" title=\"" + DictExpressionBuilderSystem.Translate("message.UserEmailConfirm") + "\">{1}</a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = encryptedTicket, Email = user.Email }, Request.Url.Scheme));
            m.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(mailServer);
            smtp.Credentials = new System.Net.NetworkCredential(EmailAdress, Password);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            return RedirectToAction("Confirm", "Account", new { Email = user.Email });
        }


        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        {

            if (Token != null)
            {
                using (LoginLogic login = new LoginLogic())
                {
                    FormsAuthenticationTicket formsTicket = FormsAuthentication.Decrypt(Token);
                    string name = formsTicket.Name;
                    string schoolName = tblSystemLogic.getSystemValueByKey("schoolName").strValue;
                    if (!formsTicket.Expired && login.IsExist(Email) && formsTicket.Name == Email && formsTicket.UserData == schoolName)//take from db
                    {
                        await SignInAsync(Email);
                        return RedirectToAction("login");
                    }
                    else
                    {

                        if (login.IsExist(Email))
                        {


                            if (formsTicket.Expired && !login.checkIfregisterd(name))
                                LoginLogic.deleteByEmail(name);
                            return RedirectToAction("Error");
                            //expierd- delete data from DB - register again
                            // return to registration page

                        }
                        return RedirectToAction("Error");
                    }
                }
            }
            else
            {
                return RedirectToAction("error");
            }
        }
        public async Task<bool> SignInAsync(string Email)//just example
        {
            await Task.Delay(1);
            return LoginLogic.confirmEmail(Email);


        }
        public ActionResult Error()
        {
            return View();
        }

    }
}

