using System.Collections.Generic;
using System.Linq;
using Aurora.Core.Data;
using Aurora.SMS.Data;
using Aurora.SMS.EFModel;

namespace Aurora.SMS.Service
{
    public interface ITemplateFieldServices
    {
        /// <summary>
        ///     Return all fileds ordered 1)GroupName 2)Name
        /// </summary>
        /// <returns></returns>
        IEnumerable<TemplateField> GetAllTemplateFields();
    }

    public class TemplateFieldServices : DbServiceBase<SMSDb>, ITemplateFieldServices
    {
        /// <summary>
        ///     Primary constructor.
        /// </summary>
        /// <param name="db">It is fine to pass the dbcontext here</param>
        public TemplateFieldServices(SMSDb db) : base(db)
        {
        }

        public IEnumerable<TemplateField> GetAllTemplateFields()
        {
            return DbContext.TemplateFields.OrderBy(x => x.GroupName).ThenBy(x => x.Name).ToArray();
        }
    }
}
