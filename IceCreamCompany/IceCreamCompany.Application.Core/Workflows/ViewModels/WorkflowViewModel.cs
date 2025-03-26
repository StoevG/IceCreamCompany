namespace IceCreamCompany.Application.Core.Workflows.ViewModels
{
    public class WorkflowViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string MultiExecBehavior { get; set; }
    }
}
