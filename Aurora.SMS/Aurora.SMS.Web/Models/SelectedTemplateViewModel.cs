using System.Collections.Generic;

namespace Aurora.SMS.Web.Models
{
    public class SelectedTemplateViewModel
    {
        public int SelectedTemplateId { get; set; }
        public IEnumerable<Aurora.SMS.EFModel.Template> Templates { get; set; }
    }
}