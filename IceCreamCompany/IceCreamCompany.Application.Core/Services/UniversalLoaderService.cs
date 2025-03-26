using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using IceCreamCompany.Application.Core.Interfaces;
using IceCreamCompany.Application.Core.Workflows.ViewModels;
using IceCreamCompany.Domain.Constants;
using IceCreamCompany.Domain.Entities.Common;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace IceCreamCompany.Application.Core.Services
{
    public class UniversalLoaderService : IUniversalLoaderService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IDistributedCache _cache;

        public UniversalLoaderService(
            HttpClient httpClient,
            IConfiguration config,
            IDistributedCache cache)
        {
            _httpClient = httpClient;
            _config = config;
            _cache = cache;
        }

        private async Task EnsureTokenAsync()
        {
            var token = await _cache.GetStringAsync(GlobalConstants.TokenCacheKey);
            var expiryString = await _cache.GetStringAsync(GlobalConstants.TokenExpiryCacheKey);

            if (!string.IsNullOrEmpty(token) &&
                !string.IsNullOrEmpty(expiryString) &&
                DateTime.TryParse(expiryString, out var expiry) &&
                DateTime.Now < expiry)
            {
                SetAuthorizationHeader(token);
                return;
            }

            token = await FetchAndCacheNewTokenAsync();
            SetAuthorizationHeader(token);
        }

        private void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<string> FetchAndCacheNewTokenAsync()
        {
            var credentials = new
            {
                apiCompanyId = _config[GlobalConstants.CompanyIdKey],
                apiUserId = _config[GlobalConstants.UserIdKey],
                apiUserSecret = _config[GlobalConstants.UserSecretKey]
            };

            var response = await _httpClient.PostAsJsonAsync(GlobalConstants.AuthenticateEndpoint, credentials);
            response.EnsureSuccessStatusCode();

            var rawToken = await response.Content.ReadAsStringAsync();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(rawToken);

            DateTime expiry = DateTime.UtcNow.AddMinutes(GlobalConstants.DefaultTokenExpiryMinutes);
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if (expClaim != null && long.TryParse(expClaim.Value, out var expUnix))
            {
                expiry = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime.AddSeconds(-GlobalConstants.TokenExpiryBufferSeconds);
            }

            await _cache.SetStringAsync(GlobalConstants.TokenCacheKey, rawToken);
            await _cache.SetStringAsync(GlobalConstants.TokenExpiryCacheKey, expiry.ToString("O"));

            return rawToken;
        }

        public async Task<ServiceResult<List<WorkflowViewModel>>> GetWorkflowsAsync()
        {
            await EnsureTokenAsync();

            var response = await _httpClient.GetAsync(GlobalConstants.WorkflowsEndpoint);
            response.EnsureSuccessStatusCode();

            var workflows = await response.Content.ReadFromJsonAsync<List<WorkflowViewModel>>();
            workflows[0].Id = 2;
            return ServiceResult.Success(workflows);
        }

        public async Task<ServiceResult> RunWorkflowAsync(int workflowId)
        {
            await EnsureTokenAsync();

            var response = await _httpClient.PostAsync($"{GlobalConstants.WorkflowsEndpoint}/{workflowId}/run", null);
            return response.IsSuccessStatusCode
                ? new ServiceResult()
                : ServiceResult.Failed(ServiceError.CustomMessage(ErrorMessages.FailedToRunWorkflow));
        }
    }
}
