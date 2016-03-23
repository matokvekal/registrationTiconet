using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic;

namespace Business_Logic
{
    public class baseLogic : IDisposable
    {
        private BusProjectEntities _db = new BusProjectEntities();

        protected BusProjectEntities DB
        {
            get
            {
                return _db;
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }


    }
}


