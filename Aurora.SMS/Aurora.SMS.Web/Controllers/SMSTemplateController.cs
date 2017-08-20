using Aurora.SMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Aurora.SMS.Web.Controllers
{
    public class SMSTemplateController : Controller
    {
        private readonly ITemplateServices _templateServices;

        public SMSTemplateController(ITemplateServices templateServices)
        {
            _templateServices = templateServices;
        }

        // GET: SMSTemplate
        public ViewResult Index()
        {
            return View(_templateServices.GetAll());
        }
        [System.Obsolete("Only for UI test")]
        public ViewResult CreateEdit()
        {
            return View("CreateEdit", new Models.SmsTemplate.SmsTemplateViewModel());
        }

        [HttpPost]
        public ActionResult CreateEdit(Models.SmsTemplate.SmsTemplateViewModel vm)
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.CreateMap<Models.SmsTemplate.SmsTemplateViewModel, EFModel.Template>());
            // Demo using FluentValidation
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            //var regExp = "<div class=\"alert alert-dismissible alert-success\" contenteditable=\"false\" style=\"display:inline-block\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\">×</button><span>" + templateField.Name + "</span></div>";
            // strip any additional spaces that Js might add in the experssion
            vm.Text = Regex.Replace(vm.Text, "alert\\s*-\\s*dismissible", "alert-dismissible");
            vm.Text = Regex.Replace(vm.Text, "alert\\s*-\\s*success", "alert-success");
            vm.Text = Regex.Replace(vm.Text, "data\\s*-\\s*dismiss", "alert-dismiss");
            if (vm.Id != 0)
            {
                _templateServices.Update(AutoMapper.Mapper.Map<EFModel.Template>(vm));
            }
            else
            {
                _templateServices.CreateTemplate(AutoMapper.Mapper.Map<EFModel.Template>(vm));
            }
            return RedirectToAction("Index");

        }

        /// <summary>
        /// Dispays the form by injecting a new template object
        /// </summary>
        /// <returns></returns>
        public ViewResult Create()
        {
            return View("CreateEdit", new Models.SmsTemplate.SmsTemplateViewModel());
        }

        public ViewResult Edit(int id)
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.CreateMap<EFModel.Template, Models.SmsTemplate.SmsTemplateViewModel>());
            return View("CreateEdit", AutoMapper.Mapper.Map<Models.SmsTemplate.SmsTemplateViewModel>(_templateServices.GetById(id)));
        }

    }
}