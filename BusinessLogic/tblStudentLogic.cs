using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic;

namespace Business_Logic
{
    public class tblStudentLogic:baseLogic
    {
        public List<tblStudent> GetStudentByFamilyIdAndYear(int familyId)
        {
            try
            {
                BusProjectEntities db = new BusProjectEntities();
                List<tblStudent> c = db.tblStudents.Where(x => x.familyId == familyId).ToList();
                return c;
            }
            catch
            {
                return null;
            }

        }
        public List<tblStudent> GetStudentByFamilyIdAndYear(int familyId,int Year)
        {
            try
            {
                BusProjectEntities db = new BusProjectEntities();
                List<tblStudent> c = db.tblStudents.Where(x => x.familyId == familyId).Where(x=>x.yearRegistration==Year).ToList();
                return c;
            }
            catch
            {
                return null;
            }

        }
    }
}
