using AutoMapper;
using IceCreamCompany.Application.Core.Services;
using IceCreamCompany.Application.Core.Workflows.ViewModels;
using IceCreamCompany.Domain.Constants;
using IceCreamCompany.Domain.Entities;
using IceCreamCompany.Domain.Interfaces;
using Moq;

public class WorkflowServiceTests
{
    private readonly Mock<IWorkflowRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly WorkflowService _service;

    public WorkflowServiceTests()
    {
        _service = new WorkflowService(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetWorkflowByIdAsync_ReturnsNotFound_WhenWorkflowIsNull()
    {
        _repoMock.Setup(r => r.GetAsync(It.IsAny<int>()))
                 .ReturnsAsync((Workflow)null);

        var result = await _service.GetWorkflowByIdAsync(10);

        Assert.False(result.Succeeded);
        Assert.Equal(ErrorMessages.NotFound, result.Error.Message);
    }

    [Fact]
    public async Task AddWorkflowAsync_ShouldMapAndSaveWorkflow()
    {
        var viewModel = new WorkflowViewModel { Name = "New Workflow" };
        var entity = new Workflow();

        _mapperMock.Setup(m => m.Map<Workflow>(viewModel)).Returns(entity);
        _mapperMock.Setup(m => m.Map<WorkflowViewModel>(It.IsAny<Workflow>())).Returns(viewModel);

        var result = await _service.AddWorkflowAsync(viewModel);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<Workflow>()), Times.Once);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task DeleteWorkflowAsync_DeletesSuccessfully()
    {
        var result = await _service.DeleteWorkflowAsync(42);

        _repoMock.Verify(r => r.DeleteAsync(42), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);

        Assert.True(result.Succeeded);
    }
}
