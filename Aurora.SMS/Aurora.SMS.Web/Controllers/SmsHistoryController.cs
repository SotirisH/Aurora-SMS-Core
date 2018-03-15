using Aurora.SMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace Aurora.SMS.Web.Controllers
{
    public class SmsHistoryController : Controller
    {
        private readonly ISMSServices _smsServices;
        public SmsHistoryController(ISMSServices smsServices)
        {
            _smsServices = smsServices;
        }
        // GET: SmsHistory
        public ActionResult Index()
        {
            return View(new Models.SmsHistory.SmsHistoryViewModel());
        }
        [HttpPost]
        public ActionResult Index(Models.SmsHistory.SmsHistoryViewModel vm)
        {
            var h= _smsServices.GetHistory(vm.Criteria);
            vm.HistoryResults = _smsServices.GetHistory(vm.Criteria);
            return View(vm);
        }
    }
}