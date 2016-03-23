using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ticonet
{
    public class accountController : Controller
    {
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
    }


    //new registration
    //1 - insert school registration kod -check by js
    //2 - if ok - insert email - check not exist,check by regex email ok,  js
    //3 insert password - check regex  8digit char and digit.
    //4 insert password second tiome - check consistence - js


}

     