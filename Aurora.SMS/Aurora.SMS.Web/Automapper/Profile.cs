using AutoMapper;

namespace Aurora.SMS.Web.Automapper
{
    public class TemplateProfile : Profile
    {
        public TemplateProfile()
        {
            CreateMap<Models.SmsTemplate.SmsTemplateViewModel, EFModel.Template>().ReverseMap();
        }
    }
}
