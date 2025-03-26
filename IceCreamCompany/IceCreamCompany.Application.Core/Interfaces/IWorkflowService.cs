using IceCreamCompany.Application.Core.Interfaces.Abstract;
using IceCreamCompany.Application.Core.Workflows.ViewModels;
using IceCreamCompany.Domain.Entities.Common;

namespace IceCreamCompany.Application.Core.Interfaces
{
    public interface IWorkflowService : IService
    {
        Task<ServiceResult<IEnumerable<WorkflowViewModel>>> GetAllWorkflowsAsync();

        Task<ServiceResult<WorkflowViewModel>> GetWorkflowByIdAsync(int workflowId);

        Task<ServiceResult<WorkflowViewModel>> AddWorkflowAsync(WorkflowViewModel workflow);

        Task<ServiceResult<WorkflowViewModel>> UpdateWorkflowAsync(int workflowId, WorkflowViewModel workflow);

        Task<ServiceResult> DeleteWorkflowAsync(int workflowId);
    }
}
