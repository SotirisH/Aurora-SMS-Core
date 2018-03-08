using Aurora.SMS.AWS.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aurora.SMS.Web
{
    public class AutoMapperAWSProfile : Profile
    {
        public AutoMapperAWSProfile()
        {
            CreateMap<SMSMessage, EFModel.SMSHistory>();
        }
    }
}
