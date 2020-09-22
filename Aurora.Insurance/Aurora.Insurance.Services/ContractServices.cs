using Aurora.Core.Data;
using Aurora.Insurance.Data;
using Aurora.Insurance.EFModel;
using Aurora.Insurance.Services.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Aurora.Insurance.Services
{
    public interface IContractServices
    {
        /// <summary>
        ///     The main service that will return the contacs based on the given criteia
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<ContractDTO> GetContracts(QueryCriteriaDTO criteria);
    }

    public class ContractServices : DbServiceBase<InsuranceDb>, IContractServices
    {
        public ContractServices(InsuranceDb db) : base(db)
        {
        }

        /* Creating dynamic queries used to be hard back to the old days. The necesity of a SQL query builder was stronger than ever 
         * leading a lot of developers to create one. Then entity framework and the LINQ expressions were introduced changing everything.
         * Although the problem reappeared: How to create a dynamic LNQ statement? The core of the dynamic LINQ is the expression trees.
         * Using them you can create anything you like and some devs created some nice implementations.
         * Personally i like both "PredicateBuilder" by Joseph Albahari, a pioneer in dynamic predicates and David's Belmont implementation using expression trees.
         * For more info:
         *  https://blogs.msdn.microsoft.com/meek/2008/05/02/linq-to-entities-combining-predicates/
         *  http://www.codeproject.com/Articles/1079028/Build-Lambda-Expressions-Dynamically
         *  https://msdn.microsoft.com/en-us/library/mt654267.aspx
         *  http://www.albahari.com/nutshell/linqkit.aspx
         */
        /*
         * Update 19 Aug 2017: Because all sub statements use the AND operator the IQuerable comes more handy
        */
        public IEnumerable<ContractDTO> GetContracts(QueryCriteriaDTO criteria)
        {
            // create a dynamic query
            IQueryable<Contract> queryableData = DbContext.Contracts.AsQueryable();
            queryableData.Include(c => c.Person).Include(c => c.Company).Include(c => c.Person.Phones);

            //get only mobiles
            if (!string.IsNullOrWhiteSpace(criteria.ContractNumber))
            {
                queryableData = queryableData.Where(c => c.ContractNumber == criteria.ContractNumber);
            }

            if (!string.IsNullOrWhiteSpace(criteria.PlateNumber))
            {
                queryableData = queryableData.Where(c => c.PlateNumber == criteria.PlateNumber);
            }

            if (!string.IsNullOrWhiteSpace(criteria.CompanyId))
            {
                queryableData = queryableData.Where(c => c.Company.Id == criteria.CompanyId);
            }

            // TODO: Refactor to use date ranges
            if (criteria.ExpireDateFrom.HasValue)
            {
                queryableData = queryableData.Where(c => c.ExpireDate >= criteria.ExpireDateFrom);
            }

            if (criteria.ExpireDateTo.HasValue)
            {
                queryableData = queryableData.Where(c => c.ExpireDate <= criteria.ExpireDateTo);
            }

            if (criteria.IssueDateFrom.HasValue)
            {
                queryableData = queryableData.Where(c => c.IssueDate >= criteria.IssueDateFrom);
            }

            if (criteria.IssueDateTo.HasValue)
            {
                queryableData = queryableData.Where(c => c.IssueDate <= criteria.IssueDateTo);
            }

            if (criteria.StartDateFrom.HasValue)
            {
                queryableData = queryableData.Where(c => c.StartDate >= criteria.StartDateFrom);
            }

            if (criteria.StartDateTo.HasValue)
            {
                queryableData = queryableData.Where(c => c.StartDate <= criteria.StartDateTo);
            }

            if (criteria.IsCanceled.HasValue)
            {
                queryableData = queryableData.Where(c => c.IsCanceled == criteria.IsCanceled);
            }

            if (!string.IsNullOrWhiteSpace(criteria.FirstNameStartsWith))
            {
                queryableData = queryableData.Where(c => c.Person.FirstName.StartsWith(criteria.FirstNameStartsWith));
            }

            if (!string.IsNullOrWhiteSpace(criteria.LastNameStartsWith))
            {
                queryableData = queryableData.Where(c => c.Person.LastName.StartsWith(criteria.LastNameStartsWith));
            }

            if (!string.IsNullOrWhiteSpace(criteria.CompanyId))
            {
                queryableData = queryableData.Where(c => c.Company.Id == criteria.CompanyId);
            }

            // Can we use AutoMapper here?
            IQueryable<ContractDTO> t = queryableData.Select(s =>
                new ContractDTO
                {
                    Address = s.Person.Address,
                    BirthDate = s.Person.BirthDate,
                    CanceledDate = s.CanceledDate,
                    CompanyDescription = s.Company.Description,
                    CompanyId = s.Company.Id,
                    Contractid = s.Id,
                    ContractNumber = s.ContractNumber,
                    DrivingLicenceNum = s.Person.DrivingLicenceNum,
                    ExpireDate = s.ExpireDate,
                    FatherName = s.Person.FatherName,
                    FirstName = s.Person.FirstName,
                    GrossAmount = s.GrossAmount,
                    IsCanceled = s.IsCanceled,
                    IssueDate = s.IssueDate,
                    LastName = s.Person.LastName,
                    MobileNumber = s.Person.Phones.FirstOrDefault(p => p.PhoneType == PhoneType.Mobile).Number,
                    NetAmount = s.NetAmount,
                    PersonId = s.PersonId,
                    PlateNumber = s.PlateNumber,
                    ReceiptNumber = s.ReceiptNumber,
                    StartDate = s.StartDate,
                    TaxAmount = s.TaxAmount,
                    TaxId = s.Person.TaxId,
                    ZipCode = s.Person.ZipCode
                });

            return t.ToArray();
        }
    }
}
