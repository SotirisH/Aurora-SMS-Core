using Aurora.Core.Data;
using Aurora.SMS.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Aurora.SMS.EFModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Aurora.SMS.Service
{
    public interface ITemplateServices
    {
        /// <summary>
        /// Updates a template or creates a new one if the
        /// modifies template has a reference to the SMSHostory
        /// </summary>
        /// <param name="template"></param>
        void Update(EFModel.Template template);
        /// <summary>
        /// Checks if the template has references to the SMS history table
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        bool IsTemplateUsed(int templateId);
        void DeleteTemplate(int templateId);
        void CreateTemplate(EFModel.Template template);
        IEnumerable<EFModel.Template> GetAll();
        EFModel.Template GetById(int id);

    }

    public class TemplateServices : DbServiceBase<SMSDb>, ITemplateServices
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="db">It is fine to pass the dbcontext here</param>
        public TemplateServices(SMSDb db):base(db)
        {

        }

        public void CreateTemplate(Template template)
        {
            DbContext.Add(template);
            DbContext.SaveChanges();
        }

        public void DeleteTemplate(int templateId)
        {
            var templateToDelete=DbContext.Find<Template>(templateId);
            DbContext.Remove(templateToDelete);
            DbContext.SaveChanges();
        }

        public IEnumerable<Template> GetAll()
        {
            return DbContext.Templates.ToArray();
        }

        public Template GetById(int id)
        {
            return DbContext.Templates.Single(x => x.Id == id);
        }

        public bool IsTemplateUsed(int templateId)
        {
            return DbContext.SMSHistoryRecords.Any(h => h.TemplateId == templateId);
        }
        /// <summary>
        /// Updates a template or creates a new one if the
        /// modifies template has a reference to the SMSHostory
        /// </summary>
        /// <param name="template"></param>
        public void Update(Template template)
        {
            // check if there is any reference
            if (DbContext.SMSHistoryRecords.Any(h => h.TemplateId == template.Id))
            {
                template.Id = 0;
                DbContext.Add(template);
            }
            else
            {
                DbContext.Update(template);
            }
            DbContext.SaveChanges();
        }
    }
}
