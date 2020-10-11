using Microsoft.AspNetCore.Http;
using System;

namespace Aurora.SMS.Web
{
    public class CookieHelper
    {
        private const string DefaultSmsGateWayName = "DefaultSmsGateWayName";

        private IHttpContextAccessor _contextAccessor;
        private HttpContext _context { get { return _contextAccessor.HttpContext; } }
        public CookieHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Stores the new provider into a cookie
        /// </summary>
        /// <returns></returns>
        public void SetDefaultSmsGateWay(string newProvidername)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            };
            _context.Response.Cookies.Append(DefaultSmsGateWayName, newProvidername, options);
        }

        public string GetDefaultSmsGateWay()
        {
            var ret = _context.Request.Cookies[DefaultSmsGateWayName];
            if (string.IsNullOrWhiteSpace(ret))
                return "SnailAbroad";
            return ret;
        }

    }
}
