using IceCreamCompany.Application.Core.Interfaces;
using IceCreamCompany.Application.Core.Services;
using IceCreamCompany.Application.Core.Workflows.ViewModels;
using IceCreamCompany.Domain.Entities.Common;
using Moq;

namespace IceCreamCompany.Tests
{
    public class WorkflowApiSyncServiceTests
    {
        private readonly Mock<IWorkflowService> _workflowServiceMock = new();
        private readonly Mock<IUniversalLoaderService> _universalLoaderServiceMock = new();
        private readonly WorkflowApiSyncService _service;

        public WorkflowApiSyncServiceTests()
        {
            _service = new WorkflowApiSyncService(_workflowServiceMock.Object, _universalLoaderServiceMock.Object);
        }

        [Fact]
        public async Task SyncWorkflowsFromApiAsync_ShouldAddNewWorkflows()
        {
            var apiWorkflows = new List<WorkflowViewModel>
        {
            new WorkflowViewModel { Id = 1, Name = "API Workflow", IsActive = true, MultiExecBehavior = "Once" }
        };

            _universalLoaderServiceMock.Setup(x => x.GetWorkflowsAsync())
                .ReturnsAsync(ServiceResult.Success(apiWorkflows));

            _workflowServiceMock.Setup(x => x.GetAllWorkflowsAsync())
                .ReturnsAsync(ServiceResult.Success<IEnumerable<WorkflowViewModel>>(new List<WorkflowViewModel>()));

            _workflowServiceMock.Setup(x => x.AddWorkflowAsync(It.IsAny<WorkflowViewModel>()))
                .ReturnsAsync(ServiceResult.Success(new WorkflowViewModel()));

            var result = await _service.SyncWorkflowsFromApiAsync();

            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task SyncWorkflowsFromApiAsync_ShouldDeleteStaleWorkflows()
        {
            var apiWorkflows = new List<WorkflowViewModel>(); 
            var localWorkflows = new List<WorkflowViewModel> { new() { Id = 5, Name = "Old" } };

            _universalLoaderServiceMock.Setup(x => x.GetWorkflowsAsync())
                .ReturnsAsync(ServiceResult.Success(apiWorkflows));

            _workflowServiceMock.Setup(x => x.GetAllWorkflowsAsync())
                .ReturnsAsync(ServiceResult.Success<IEnumerable<WorkflowViewModel>>(localWorkflows));

            _workflowServiceMock.Setup(x => x.DeleteWorkflowAsync(5))
                .ReturnsAsync(new ServiceResult());

            var result = await _service.SyncWorkflowsFromApiAsync();

            Assert.True(result.Succeeded);
        }
    }
}
