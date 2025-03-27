using IceCreamCompany.Application.Core.Interfaces.Abstract;
using IceCreamCompany.Domain.Entities.Common;

namespace IceCreamCompany.Application.Core.Interfaces
{
    public interface IWorkflowApiSyncService : IService
    {
        Task<ServiceResult> SyncWorkflowsFromApiAsync();
    }
}
