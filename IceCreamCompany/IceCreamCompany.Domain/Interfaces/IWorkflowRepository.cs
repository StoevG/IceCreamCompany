using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCreamCompany.Domain.Entities;
using IceCreamCompany.Domain.Interfaces.Abstract;

namespace IceCreamCompany.Domain.Interfaces
{
    public interface IWorkflowRepository : IRepository<Workflow> { }
}
