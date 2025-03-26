using IceCreamCompany.Application.Core.Interfaces;
using IceCreamCompany.Application.Core.Workflows.ViewModels;
using IceCreamCompany.Domain.Constants;
using IceCreamCompany.Domain.Entities.Common;

namespace IceCreamCompany.Application.Core.Services
{
    public class WorkflowApiSyncService : IWorkflowApiSyncService
    {
        private readonly IWorkflowService _workflowService;
        private readonly IUniversalLoaderService _universalLoaderService;

        public WorkflowApiSyncService(
            IWorkflowService workflowService,
            IUniversalLoaderService universalLoaderService)
        {
            _workflowService = workflowService;
            _universalLoaderService = universalLoaderService;
        }

        public async Task<ServiceResult> SyncWorkflowsFromApiAsync()
        {
            var apiResult = await _universalLoaderService.GetWorkflowsAsync();
            if (!apiResult.Succeeded || apiResult.Data == null)
                return ServiceResult.Failed(ServiceError.CustomMessage(ErrorMessages.FailedToRetrieveWorkflowsFromApi));

            List<WorkflowViewModel> apiWorkflows = apiResult.Data;

            var localResult = await _workflowService.GetAllWorkflowsAsync();
            if (!localResult.Succeeded || localResult.Data == null)
                return ServiceResult.Failed(ServiceError.CustomMessage(ErrorMessages.FailedToRetrieveLocalWorkflows));

            var localWorkflows = localResult.Data.ToList();
            var localWorkflowsDict = localWorkflows.ToDictionary(lw => lw.Id);

            foreach (var apiWorkflow in apiWorkflows)
            {
                if (apiWorkflow == null || apiWorkflow.Id <= 0)
                    continue;

                if (localWorkflowsDict.TryGetValue(apiWorkflow.Id, out var localWorkflow))
                {
                    if (NeedsUpdate(localWorkflow, apiWorkflow))
                    {
                        UpdateLocalWorkflow(localWorkflow, apiWorkflow);
                        var updateResult = await _workflowService.UpdateWorkflowAsync(apiWorkflow.Id, localWorkflow);
                        if (!updateResult.Succeeded)
                            return ServiceResult.Failed(ServiceError.CustomMessage(string.Format(ErrorMessages.FailedToUpdateWorkflow, apiWorkflow.Id)));
                    }
                    localWorkflowsDict.Remove(apiWorkflow.Id);
                }
                else
                {
                    var addResult = await _workflowService.AddWorkflowAsync(apiWorkflow);
                    if (!addResult.Succeeded)
                        return ServiceResult.Failed(ServiceError.CustomMessage(string.Format(ErrorMessages.FailedToAddWorkflow, apiWorkflow.Id)));
                }
            }

            foreach (var workflowToDelete in localWorkflowsDict.Values)
            {
                var deleteResult = await _workflowService.DeleteWorkflowAsync(workflowToDelete.Id);
                if (!deleteResult.Succeeded)
                    return ServiceResult.Failed(ServiceError.CustomMessage(string.Format(ErrorMessages.FailedToDeleteWorkflow, workflowToDelete.Id)));
            }

            return new ServiceResult();
        }

        private static bool NeedsUpdate(WorkflowViewModel localWorkflow, WorkflowViewModel apiWorkflow)
        {
            return !string.Equals(localWorkflow.Name, apiWorkflow.Name, StringComparison.Ordinal)
                   || localWorkflow.IsActive != apiWorkflow.IsActive
                   || !string.Equals(localWorkflow.MultiExecBehavior, apiWorkflow.MultiExecBehavior, StringComparison.Ordinal);
        }

        private static void UpdateLocalWorkflow(WorkflowViewModel localWorkflow, WorkflowViewModel apiWorkflow)
        {
            localWorkflow.Name = apiWorkflow.Name;
            localWorkflow.IsActive = apiWorkflow.IsActive;
            localWorkflow.MultiExecBehavior = apiWorkflow.MultiExecBehavior;
        }
    }
}
