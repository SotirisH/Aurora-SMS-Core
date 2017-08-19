using Aurora.Core.Data;
using Aurora.SMS.Data;
using Aurora.SMS.Service.Data;
using Aurora.SMS.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.SMS.Service
{
    public interface ITemplateFieldServices
    {
        /// <summary>
        /// Return all fileds ordered 1)GroupName 2)Name
        /// </summary>
        /// <returns></returns>
        IEnumerable<EFModel.TemplateField> GetAllTemplateFields();
    }

    public class TemplateFieldServices : DbServiceBase<SMSDb>
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="db">It is fine to pass the dbcontext here</param>
        public TemplateFieldServices(SMSDb db):base(db)
        {

        }

        public IEnumerable<EFModel.TemplateField> GetAllTemplateFields()
        {
            return DbContext.TemplateFields.OrderBy(x => x.GroupName).ThenBy(x => x.Name).ToArray();
        }
    }
}
