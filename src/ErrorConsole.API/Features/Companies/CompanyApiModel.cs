using ErrorConsole.Core.Models;
using System;

namespace ErrorConsole.API.Features.Companies
{
    public class CompanyApiModel
    {        
        public Guid CompanyId { get; set; }
        public string Name { get; set; }

        public static CompanyApiModel FromCompany(Company company)
        {
            var model = new CompanyApiModel();
            model.CompanyId = company.CompanyId;
            model.Name = company.Name;
            return model;
        }
    }
}
