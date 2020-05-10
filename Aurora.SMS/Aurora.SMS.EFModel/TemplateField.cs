using Aurora.Core.Data;

namespace Aurora.SMS.EFModel
{
    public class TemplateField : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public string DataFormat { get; set; }
    }
}
