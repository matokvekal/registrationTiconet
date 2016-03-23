using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic;

namespace Business_Logic
{
    public class tblFamilyLogic : baseLogic
    {

        public tblFamily GetFamilyById(int familyId)
        {
            try
            {
                BusProjectEntities db = new BusProjectEntities();
                return db.tblFamilies.FirstOrDefault(c => c.familyId == familyId);
            }
            catch
            {
                return null;
            }

        }

        public static bool checkEmailExist(string email)
        {
            BusProjectEntities db = new BusProjectEntities();

            return (db.tblFamilies.Any(x => x.parent1Email == email));
        }
    }
}
