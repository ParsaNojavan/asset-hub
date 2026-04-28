using Storage.Application.DTOs;
using Storage.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Storage.Infrastructure.Services
{
    public class PermissionApiClient : IPermissionApiClient
    {

        private readonly HttpClient _httpClient;

        public PermissionApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> HasPermissionAsync(Guid assetId, Guid userId)
        {
            var url = $"api/shares/check-permission?assetId={assetId}&userId={userId}";
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<PermissionResponse>();

            return data?.HasPermission ?? false;
        }
    }
}
