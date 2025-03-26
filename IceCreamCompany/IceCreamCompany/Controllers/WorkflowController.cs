using IceCreamCompany.Application.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IceCreamCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;
        private readonly IWorkflowApiSyncService _workflowApiSyncService;
        private readonly IUniversalLoaderService _universalLoaderService;

        public WorkflowsController(
            IWorkflowService workflowService,
            IWorkflowApiSyncService workflowApiSyncService,
            IUniversalLoaderService universalLoaderService)
        {
            _workflowService = workflowService;
            _workflowApiSyncService = workflowApiSyncService;
            _universalLoaderService = universalLoaderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _workflowService.GetAllWorkflowsAsync();
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkflow(int workflowId)
        {
            var result = await _workflowService.GetWorkflowByIdAsync(workflowId);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncWorkflows()
        {
            var result = await _workflowApiSyncService.SyncWorkflowsFromApiAsync();
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{workflowId}/run")]
        public async Task<IActionResult> RunWorkflow(int workflowId)
        {
            var result = await _universalLoaderService.RunWorkflowAsync(workflowId);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}