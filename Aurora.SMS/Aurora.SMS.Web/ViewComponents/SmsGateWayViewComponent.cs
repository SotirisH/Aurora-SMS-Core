using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Aurora.SMS.Web.ViewComponents
{
    public class SmsGateWayViewComponent : ViewComponent
    {
        private readonly CookieHelper _cookieHelper;
        public SmsGateWayViewComponent(CookieHelper cookieHelper)
        {
            _cookieHelper = cookieHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int maxPriority, bool isDone)
        {
            var smsGateWayName = _cookieHelper.GetDefaultSmsGateWay();
            ViewBag.SmsGateWayName = smsGateWayName;
            return await Task.FromResult<IViewComponentResult>(View());
        }
    }
}
