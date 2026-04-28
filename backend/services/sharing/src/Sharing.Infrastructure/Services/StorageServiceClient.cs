using MediatR;
using Sharing.Application.DTOs;
using Sharing.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharing.Infrastructure.Services
{
    public class StorageServiceClient : IStorageServiceClient
    {
        private readonly HttpClient _httpClient;

        public StorageServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<AssetDto?> GetAssetAsync(Guid resourceId,string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/files/{resourceId}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AssetDto>();
        }
    }
}
