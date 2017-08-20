using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aurora.SMS.Service;
using Microsoft.AspNetCore.Http;
using Aurora.SMS.Web.Models.SmsGateway;

namespace Aurora.SMS.Web.Controllers
{
    public class SmsGateWayController : Controller
    {
        private const string DefaultSmsGateWayName = "DefaultSmsGateWayName";
        private readonly ISMSServices _sMSServices;
        public SmsGateWayController(ISMSServices sMSServices)
        {
            _sMSServices = sMSServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ViewResult Change()
        {

            var smsProxies = _sMSServices.GetAllProviders();
            var smsDefaultGateWayName = Request.Cookies[DefaultSmsGateWayName];
            // if the DefaultSmsGateWayName has not been set the the first item is set as the DefaultSmsGateWayName
            if (string.IsNullOrWhiteSpace(smsDefaultGateWayName))
            {
                if (smsProxies.Any())
                {
                    smsDefaultGateWayName = smsProxies.Last().Name;
                }
            }
            SetDefaultSmsGateWay(smsDefaultGateWayName);
            

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

        public PartialViewResult SmsGateWayBlockView()
        {
            var smsGateWayName = Request.Cookies[DefaultSmsGateWayName];
            ViewBag.SmsGateWayName = smsGateWayName;
            return PartialView("_SmsGateWay");
        }

        /// <summary>
        /// Sets this proxy as default
        /// </summary>
        /// <param name="model">Generic Post load</param>
        [HttpPost]
        public RedirectToActionResult SetDefault(FormCollection model)
        {
            // Update cookie
            SetDefaultSmsGateWay( model["proxyname"]);
            return RedirectToAction("Change");
        }

        /// <summary>
        /// Stores the new provider into a cookie
        /// </summary>
        /// <returns></returns>
        private void SetDefaultSmsGateWay(string newProvidername)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append(DefaultSmsGateWayName, newProvidername, options);
        }
    }

}