using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aurora.SMS.Web
{
    public class AutoMapperWebProfile : Profile
    {
        public AutoMapperWebProfile()
        {
            CreateMap<Models.SmsTemplate.SmsTemplateViewModel, EFModel.Template>().ReverseMap();
        }
    }
}
