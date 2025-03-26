namespace IceCreamCompany.Domain.Constants
{
    public static class ErrorMessages
    {
        public const string FailedToRetrieveWorkflowsFromApi = "Failed to retrieve workflows from API.";
        public const string FailedToRetrieveLocalWorkflows = "Failed to retrieve local workflows.";
        public const string FailedToUpdateWorkflow = "Failed to update workflow with id: {0}";
        public const string FailedToAddWorkflow = "Failed to add workflow with id: {0}";
        public const string FailedToDeleteWorkflow = "Failed to delete workflow with id: {0}";

        public const string NotFound = "The specified resource was not found.";
        public const string WorkflowIsNull = "Workflow is null.";

        public const string FailedToRunWorkflow = "Failed to run workflow.";
    }
}
