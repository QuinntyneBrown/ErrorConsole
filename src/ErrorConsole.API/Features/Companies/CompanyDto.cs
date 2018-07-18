using ErrorConsole.Core.Models;
using System;

namespace ErrorConsole.API.Features.Companies
{
    public class CompanyDto
    {        
        public Guid CompanyId { get; set; }
        public string Name { get; set; }

        public static CompanyDto FromCompany(Company company)
            => new CompanyDto
            {
                CompanyId = company.CompanyId,
                Name = company.Name
            };
    }
}
