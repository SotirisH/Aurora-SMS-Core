using Microsoft.AspNetCore.Mvc;

namespace Aurora.SMS.Web.Controllers
{
    public class AuroraSMSController : Controller
    {
        // GET: AuroraSMS
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}