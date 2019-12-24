using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aurora.SMS.Web.Automapper
{
    public class TemplateProfile:Profile
    {
        public TemplateProfile()
        {
            CreateMap<Models.SmsTemplate.SmsTemplateViewModel, EFModel.Template>().ReverseMap();
        }
    }
}
