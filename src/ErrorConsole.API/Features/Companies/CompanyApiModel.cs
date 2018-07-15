using ErrorConsole.Core.Models;
using System;

namespace ErrorConsole.API.Features.Companies
{
    public class CompanyApiModel
    {        
        public Guid CompanyId { get; set; }
        public string Name { get; set; }

        public static CompanyApiModel FromCompany(Company company)
            => new CompanyApiModel
            {
                CompanyId = company.CompanyId,
                Name = company.Name
            };
    }
}
