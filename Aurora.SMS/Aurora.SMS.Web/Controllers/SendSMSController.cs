using Aurora.Insurance.Services;
using Aurora.SMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Aurora.SMS.Web.Controllers
{
    public class SendSMSController : Controller
    {
        private readonly CookieHelper _cookieHelper;
        private readonly ICompanyServices _companyServices;
        private readonly IContractServices _contractServices;
        private readonly ITemplateServices _templateServices;
        private readonly ISMSServices _smsServices;
        private readonly SessionHelper _sessionHelper;

        public SendSMSController(ICompanyServices companyServices,
                                IContractServices contractServices,
                                ITemplateServices templateServices,
                                ISMSServices smsServices,
                                SessionHelper sessionHelper,
                                CookieHelper cookieHelper)
        {
            _companyServices = companyServices;
            _templateServices = templateServices;
            _smsServices = smsServices;
            _sessionHelper = sessionHelper;
            _cookieHelper = cookieHelper;
            _contractServices = contractServices;
        }
        // GET: SendSMS
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult SelectCustomers()
        {
            var mock = new Models.CustomerSelectionViewModel();
            mock.Companies = _companyServices.GetAll();
            mock.Contracts = new List<Insurance.Services.DTO.ContractDTO>();
            return View(mock);
        }

        [HttpPost]
        public ViewResult SelectCustomers(Models.CustomerSelectionViewModel vm)
        {
            var results = _contractServices.GetContracts(vm.Criteria);
            vm.Companies = _companyServices.GetAll();
            vm.Contracts = results;
            return View(vm);
        }


        [HttpPost]
        public ActionResult AdvancedToSelectTemplate(Models.CustomerSelectionViewModel vm)
        {
            // Store Criteria into the session
            _sessionHelper.Criteria = vm.Criteria;
            return RedirectToAction("SelectTemplate");
        }

        public ViewResult SelectTemplate()
        {
            var vm = new Models.SelectedTemplateViewModel();
            vm.Templates = _templateServices.GetAll();
            return View(vm);
        }

        [HttpPost]
        public ActionResult SelectTemplate(Models.SelectedTemplateViewModel vm)
        {
            _sessionHelper.SelectedTemplateId = vm.SelectedTemplateId;
            return RedirectToAction("Preview");
        }

        public ViewResult Preview()
        {
            Insurance.Services.DTO.QueryCriteriaDTO criteria = _sessionHelper.Criteria as Insurance.Services.DTO.QueryCriteriaDTO;
            int selectedTemplateId = _sessionHelper.SelectedTemplateId;

            var recepients = _contractServices.GetContracts(criteria);
            var previewMessages = _smsServices.ConstructSMSMessages(recepients, selectedTemplateId);
            return View(previewMessages);
        }

        public ViewResult BulkSmsResult()
        {
            Insurance.Services.DTO.QueryCriteriaDTO criteria = _sessionHelper.Criteria as Insurance.Services.DTO.QueryCriteriaDTO;
            int selectedTemplateId = _sessionHelper.SelectedTemplateId;
            var recepients = _contractServices.GetContracts(criteria);
            var previewMessages = _smsServices.ConstructSMSMessages(recepients, selectedTemplateId);
            var sessionId = _smsServices.QueueForSend(previewMessages, _cookieHelper.GetDefaultSmsGateWay());
            ViewBag.SessionId = sessionId;
            return View();
        }


    }
}