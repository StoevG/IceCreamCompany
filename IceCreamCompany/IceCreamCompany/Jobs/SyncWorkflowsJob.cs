using Quartz;
using IceCreamCompany.Application.Core.Interfaces;

namespace IceCreamCompany.Api.Jobs
{
    public class SyncWorkflowsJob : IJob
    {
        private readonly IWorkflowApiSyncService _workflowApiSyncService;

        public SyncWorkflowsJob(IWorkflowApiSyncService workflowApiSyncService)
        {
            _workflowApiSyncService = workflowApiSyncService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _workflowApiSyncService.SyncWorkflowsFromApiAsync();
        }
    }
}