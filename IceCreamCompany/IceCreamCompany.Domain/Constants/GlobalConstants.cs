namespace IceCreamCompany.Domain.Constants
{
    public class GlobalConstants
    {
        public const string TokenCacheKey = "UniversalLoaderToken";
        public const string TokenExpiryCacheKey = "UniversalLoaderTokenExpiry";
        public const string AuthenticateEndpoint = "/authenticate";
        public const string WorkflowsEndpoint = "/workflows";
        public const int DefaultTokenExpiryMinutes = 15;
        public const int TokenExpiryBufferSeconds = 30;

        public const string CompanyIdKey = "UniversalLoader:CompanyId";
        public const string UserIdKey = "UniversalLoader:UserId";
        public const string UserSecretKey = "UniversalLoader:UserSecret";
        public const string BaseUrl = "UniversalLoader:ApiBaseUrl";

        public const string DefaultConnectionStringName = "DefaultConnection";
        public const string RedisConnectionStringName = "RedisConnection";

        public const string RedisInstanceName = "UniversalLoader:";

        public const string SyncWorkflowsJobKey = "SyncWorkflowsJob";
        public const string SyncWorkflowsJobTriggerKey = "SyncWorkflowsJob-trigger";
    }
}
