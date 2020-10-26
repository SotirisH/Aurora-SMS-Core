using Aurora.SMS.Service;
using Aurora.SMS.Web.Models.SmsGateway;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aurora.SMS.Web.Controllers
{
    public class SmsGateWayController : Controller
    {

        private readonly ISMSServices _sMSServices;
        private readonly CookieHelper _cookieHelper;
        public SmsGateWayController(ISMSServices sMSServices,
                                    CookieHelper cookieHelper)
        {
            _sMSServices = sMSServices;
            _cookieHelper = cookieHelper;
        }

        public ViewResult Change()
        {

            var smsProxies = _sMSServices.GetAllProviders();
            var smsDefaultGateWayName = _cookieHelper.GetDefaultSmsGateWay();
            // if the DefaultSmsGateWayName has not been set the the first item is set as the DefaultSmsGateWayName
            if (string.IsNullOrWhiteSpace(smsDefaultGateWayName))
            {
                if (smsProxies.Any())
                {
                    smsDefaultGateWayName = smsProxies.Last().Name;
                }
            }
            _cookieHelper.SetDefaultSmsGateWay(smsDefaultGateWayName);


            var vm = new List<SmsGatewayProxyViewModel>();
            foreach (var item in smsProxies)
            {
                SmsGatewayProxyViewModel tmp = new SmsGatewayProxyViewModel();
                tmp.LogoUrl = item.LogoUrl;
                tmp.Name = item.Name;
                tmp.IsDefault = (smsDefaultGateWayName == item.Name);
                tmp.UserName = item.UserName;
                tmp.Pasword = item.PassWord;
                tmp.SiteUrl = item.Url;
                vm.Add(tmp);
            }

            return View(vm);
        }



        /// <summary>
        /// Sets this proxy as default
        /// </summary>
        /// <param name="model">Generic Post load</param>
        [HttpPost]
        public RedirectToActionResult SetDefault(FormCollection model)
        {
            // Update cookie
            _cookieHelper.SetDefaultSmsGateWay(model["proxyname"]);
            return RedirectToAction("Change");
        }

        [HttpPost]
        public ActionResult Save(string proxyName, string userName, string password)
        {
            try
            {
                var efproxy = _sMSServices.GetAllProviders().First(m => m.Name == proxyName);
                // update only username & password
                efproxy.UserName = userName;
                efproxy.PassWord = password;
                _sMSServices.SaveProxy(efproxy);
                var availableCredits = _sMSServices.GetAvailableCredits(proxyName);
                return Json(new { Error = false, Message = availableCredits });
            }
            catch (Exception ex)
            {
                return Json(new { Error = true, Message = ex.Message });
            }

        }

        public ActionResult TestProxy(string proxyName, string userName, string password)
        {
            try
            {
                var availableCredits = _sMSServices.GetAvailableCredits(proxyName, userName, password);
                return Json(new { Error = false, Message = availableCredits });
            }
            catch (Exception ex)
            {
                return Json(new { Error = true, Message = ex.Message });
            }
        }


        /// <summary>
        /// Returns the available credits of the active SmsProxy
        /// </summary>
        /// <returns></returns>
        public string GetAvailableCredits()
        {

            string defaultSmsGateWayName = _cookieHelper.GetDefaultSmsGateWay();
            if (!string.IsNullOrWhiteSpace(defaultSmsGateWayName))
            {
                return _sMSServices.GetAvailableCredits(defaultSmsGateWayName);
            }
            return "?";
        }


    }

}
