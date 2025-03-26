using IceCreamCompany.Application.Core.Workflows.ViewModels;
using IceCreamCompany.Domain.Entities.Common;

namespace IceCreamCompany.Application.Core.Interfaces
{
    public interface IUniversalLoaderService
    {
        Task<ServiceResult<List<WorkflowViewModel>>> GetWorkflowsAsync();
        Task<ServiceResult> RunWorkflowAsync(int workflowId);
    }
}
