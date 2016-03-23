using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Business_Logic;

namespace ticonet
{
    public class FamilyController : Controller
    {

        public ActionResult Index(int id,int? year)//id=familyId
        {
            using (tblFamilyLogic family = new tblFamilyLogic())
            {
                using (tblStudentLogic students = new tblStudentLogic())
                {
                    ViewBag.Years = tblYearsLogic.GetYears();
                    tblFamily c = family.GetFamilyById(id);
                    if (c != null)
                    {List<tblStudent> s;
                        if(!year.HasValue)
                         s = students.GetStudentByFamilyIdAndYear(id);
                        else
                             s = students.GetStudentByFamilyIdAndYear(id,year.Value);

                        familyViewModel vm = new familyViewModel()
                        {
                            EditableTblFamily = c,
                            students = s
                        };

                        return View(vm);
                    }
                    return null;

                }
            }
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public object create(string email)
        {
            //check email not null
            //check email not exist
            //create emptyform + insert email

            if (!string.IsNullOrEmpty(email) && !tblFamilyLogic.checkEmailExist(email) && Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")  )
            {
                 using (tblFamilyLogic family = new tblFamilyLogic())
                 {
                     tblFamily c = new tblFamily();
                     c.parent1Email = email;
                     c.iAgree = false;
                     c.parent1GetAlertBycell= false;
                     c.parent1GetAlertByEmail = false;
                     c.parent1EmailConfirm = false;
                     c.parent1CellConfirm = false;
                     c.parent2GetAlertBycell = false;
                     c.parent2GetAlertByEmail = false;
                     c.parent2EmailConfirm = false;
                     c.parent2CellConfirm = false;
                     c.paymentOk = false;
                     c.date = DateTime.Today;
                     return View(c);
                
                 }
            }
            else
                return Redirect("~/account/unAutorise");//real check is by Js in client side- if we here there is asecurity problem!
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public object create(tblFamily c)
        {
            

            if (c!=null)
            {
               
                
                    return View(c);

            }
            else
                return Redirect("~/account/unAutorise");//real check is by Js in client side- if we here there is asecurity problem!
        }

    }
}