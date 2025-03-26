using AutoMapper;
using IceCreamCompany.Application.Core.Interfaces;
using IceCreamCompany.Application.Core.Workflows.ViewModels;
using IceCreamCompany.Domain.Constants;
using IceCreamCompany.Domain.Entities;
using IceCreamCompany.Domain.Entities.Common;
using IceCreamCompany.Domain.Interfaces;

namespace IceCreamCompany.Application.Core.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IMapper _mapper;

        public WorkflowService(IWorkflowRepository workflowRepository, IMapper mapper)
        {
            _workflowRepository = workflowRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<IEnumerable<WorkflowViewModel>>> GetAllWorkflowsAsync()
        {
            var workflows = _workflowRepository.GetAll().ToList();
            var viewModels = _mapper.Map<List<WorkflowViewModel>>(workflows);
            return ServiceResult.Success<IEnumerable<WorkflowViewModel>>(viewModels);
        }

        public async Task<ServiceResult<WorkflowViewModel>> GetWorkflowByIdAsync(int workflowId)
        {
            var workflow = await _workflowRepository.GetAsync(workflowId);
            if (workflow == null)
                return ServiceResult.Failed<WorkflowViewModel>(ServiceError.CustomMessage(ErrorMessages.NotFound));

            var viewModel = _mapper.Map<WorkflowViewModel>(workflow);
            return ServiceResult.Success(viewModel);
        }

        public async Task<ServiceResult<WorkflowViewModel>> AddWorkflowAsync(WorkflowViewModel workflowViewModel)
        {
            if (workflowViewModel == null)
                return ServiceResult.Failed<WorkflowViewModel>(ServiceError.CustomMessage(ErrorMessages.WorkflowIsNull));

            var workflow = _mapper.Map<Workflow>(workflowViewModel);
            workflow.CreatedDate = DateTime.UtcNow;

            await _workflowRepository.AddAsync(workflow);
            await _workflowRepository.SaveChangesAsync();

            var createdViewModel = _mapper.Map<WorkflowViewModel>(workflow);
            return ServiceResult.Success(createdViewModel);
        }

        public async Task<ServiceResult<WorkflowViewModel>> UpdateWorkflowAsync(int workflowId, WorkflowViewModel workflowViewModel)
        {
            if (workflowViewModel == null)
                return ServiceResult.Failed<WorkflowViewModel>(ServiceError.CustomMessage(ErrorMessages.WorkflowIsNull));

            var workflow = await _workflowRepository.GetAsync(workflowId);
            if (workflow == null)
                return ServiceResult.Failed<WorkflowViewModel>(ServiceError.CustomMessage(ErrorMessages.NotFound));

            workflow.WorkflowName = workflowViewModel.Name;
            workflow.IsActive = workflowViewModel.IsActive;
            workflow.MultiExecBehavior = workflowViewModel.MultiExecBehavior;

            _workflowRepository.Update(workflow);
            await _workflowRepository.SaveChangesAsync();

            var updatedViewModel = _mapper.Map<WorkflowViewModel>(workflow);
            return ServiceResult.Success(updatedViewModel);
        }

        public async Task<ServiceResult> DeleteWorkflowAsync(int workflowId)
        {
            await _workflowRepository.DeleteAsync(workflowId);
            await _workflowRepository.SaveChangesAsync();
            return new ServiceResult();
        }
    }
}
