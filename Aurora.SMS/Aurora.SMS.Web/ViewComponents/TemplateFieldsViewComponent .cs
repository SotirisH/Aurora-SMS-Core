using Aurora.SMS.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aurora.SMS.Web.ViewComponents
{

    /// <summary>
    /// ViewComponent for the template fields
    /// </summary>
    /// <remarks>https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components</remarks>
    public class TemplateFieldsViewComponent : ViewComponent
    {
        private readonly ITemplateFieldServices _templateFieldServices;
        public TemplateFieldsViewComponent(ITemplateFieldServices templateFieldServices)
        {
            _templateFieldServices = templateFieldServices;
        }

        public async Task<IViewComponentResult> InvokeAsync(int maxPriority, bool isDone)
        {
            // TODO: Create an async version of GetAllTemplateFields
            return View( _templateFieldServices.GetAllTemplateFields());
        }
    }
}
