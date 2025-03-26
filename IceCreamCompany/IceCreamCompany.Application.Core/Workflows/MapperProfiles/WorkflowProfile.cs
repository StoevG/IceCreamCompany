using AutoMapper;
using IceCreamCompany.Application.Core.Workflows.ViewModels;
using IceCreamCompany.Domain.Entities;

namespace IceCreamCompany.Application.Core.Workflows.MapperProfiles
{
    public class WorkflowProfile : Profile
    {
        public WorkflowProfile()
        {
            CreateMap<Workflow, WorkflowViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.WorkflowId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.WorkflowName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.MultiExecBehavior, opt => opt.MapFrom(src => src.MultiExecBehavior));

            CreateMap<WorkflowViewModel, Workflow>()
                .ForMember(dest => dest.WorkflowId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.WorkflowName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.MultiExecBehavior, opt => opt.MapFrom(src => src.MultiExecBehavior));
        }
    }
}
