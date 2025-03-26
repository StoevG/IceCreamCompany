using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCreamCompany.Application.Core.Interfaces.Abstract;
using IceCreamCompany.Domain.Entities.Common;

namespace IceCreamCompany.Application.Core.Interfaces
{
    public interface IWorkflowApiSyncService : IService
    {
        Task<ServiceResult> SyncWorkflowsFromApiAsync();
    }
}
