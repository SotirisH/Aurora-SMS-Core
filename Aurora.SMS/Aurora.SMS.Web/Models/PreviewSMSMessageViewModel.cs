using System.Collections.Generic;

namespace Aurora.SMS.Web.Models
{
    public class PreviewSMSMessageViewModel
    {

        public IEnumerable<Aurora.SMS.Service.DTO.SMSMessageDTO> SmsMessageList { get; set; }
    }
}