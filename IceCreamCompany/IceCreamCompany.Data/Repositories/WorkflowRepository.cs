using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCreamCompany.Data.Repositories.Abstracts;
using IceCreamCompany.Domain.Entities;
using IceCreamCompany.Domain.Interfaces;

namespace IceCreamCompany.Data.Repositories
{
    public class WorkflowRepository : BaseRepository<Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(IceCreamCompanyContext context) : base(context) { }
    }
}
