using Aurora.Core.Data;
using Microsoft.AspNetCore.Http;

namespace Aurora.SMS.Web
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://long2know.com/2016/07/accessing-httpcontext-in-asp-net-core/</remarks>
    public class CurrentUserService : ICurrentUserService
    {
        private IHttpContextAccessor _contextAccessor;
        private HttpContext _context { get { return _contextAccessor.HttpContext; } }
        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetCurrentUser()
        {
            var userName = "Anonymous";
            // Figure out the user's identity
            if (_context != null)
            {
                if (_context.User != null)
                {
                    var identity = _context.User.Identity;

                    if (identity != null && identity.IsAuthenticated)
                    {
                        userName = identity.Name;
                    }
                }
            }
            return userName;
        }
    }
}
