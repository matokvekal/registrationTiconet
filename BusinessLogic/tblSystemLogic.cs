using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic;

namespace Business_Logic
{
    public class tblSystemLogic : baseLogic
    {



        public static tblSystem getSystemValueByKey(string key)
        {
            try
            {
                BusProjectEntities db = new BusProjectEntities();
                return db.tblSystems.FirstOrDefault(c => c.strKey == key);
            }
            catch
            {
                return null;
            }
        }
     
    }
}
