using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Business_Logic;
//this classis temporary used for the alert at the bus project for inserting to BD all the alerts
namespace ticonet
{
    public class alertApiController : ApiController
    {
        public HttpResponseMessage PostMessage(string Alert1,string Alert2,string Alert3,string LastLocationTime,string BusLineNumber,string DriverName,string PlateNumber)
        {
            try
            {
                tblAlertsQueue c = new tblAlertsQueue();
                c.messageA = Alert1;
                c.messageB = Alert2;
                c.messageC = Alert3;
                tblAlertsQueueLogic.create(c);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception)
            {

                return new  HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
       
                          