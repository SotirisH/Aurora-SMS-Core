using Microsoft.AspNetCore.Http;
using System;

namespace Aurora.SMS.Web
{
    public class CookieHelper
    {
        private const string DefaultSmsGateWayName = "DefaultSmsGateWayName";

        private HttpContext _contextAccessor { get; set; }
        public CookieHelper(HttpContext contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Stores the new provider into a cookie
        /// </summary>
        /// <returns></returns>
        public void SetDefaultSmsGateWay(string newProvidername)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            _contextAccessor.Response.Cookies.Append(DefaultSmsGateWayName, newProvidername, options);
        }

        public string GetDefaultSmsGateWay()
        {
            return _contextAccessor.Request.Cookies[DefaultSmsGateWayName];
        }

    }
}
