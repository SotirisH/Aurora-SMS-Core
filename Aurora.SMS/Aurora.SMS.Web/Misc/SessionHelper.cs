using Aurora.Insurance.Services.DTO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Aurora.SMS.Web
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state</remarks>
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
    }

    /// <summary>
    /// All the interaction with the session is done by this helper
    /// </summary>
    /// <remarks>
    /// This approach has several advantages:
    /// 1)it saves you from a lot of type-casting
    /// 2)you don't have to use hard-coded session keys throughout your application (e.g. Session["loginId"]
    /// 3)you can document your session items by adding XML doc comments on the properties of MySession
    /// 4)you can initialize your session variables with default values(e.g.assuring they are not null)
    /// </remarks>
    public class SessionHelper
    {
        /// <summary>
        /// The selected Template Id when we run the wizzard to send SMS
        /// </summary>
        public int SelectedTemplateId { get; set; }
        public QueryCriteriaDTO Criteria { get; set; }

        private IHttpContextAccessor _contextAccessor;
        private HttpContext _context { get { return _contextAccessor.HttpContext; } }
        public SessionHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        // Gets the current session.
        public SessionHelper Current()
        {
            {
                var session = _context.Session.Get<SessionHelper>("__MySession__");
                if (session == null)
                {
                    session = new SessionHelper(_contextAccessor);
                    session.Criteria = new QueryCriteriaDTO();
                    _context.Session.Set("__MySession__", session);
                }
                return session;
            }
        }


    }

}