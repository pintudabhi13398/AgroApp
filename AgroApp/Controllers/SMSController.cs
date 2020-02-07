using Nexmo.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities;
using Common;

namespace AgroApp.Controllers
{
    public class SMSController : BaseController
    {
        // GET: SMS
        public ActionResult Index()
        {
            return View();
        }
       [HttpPost]
        public ActionResult SendSMS(MessageModel model)
        {
            try
            {
                var client = new Client(creds: new Nexmo.Api.Request.Credentials
                {
                    ApiKey = "85b8f47c",
                    ApiSecret = "5xk0i2CktrK63Nw4"
                });
                var results = client.SMS.Send(request: new SMS.SMSRequest
                {
                    from = "Agro App By Pintu Dabhi",
                    to = model.To,
                    text = model.ContentMsg
                });
                return Json(new { Message = results.messages, IsError = Convert.ToString((int)Enums.NotifyType.Success) });
            }
            catch(Exception ex)
            {
                return Json(new { Message = ex.Message, IsError = Convert.ToString((int)Enums.NotifyType.Success) });
            }

            




        }
    }
}