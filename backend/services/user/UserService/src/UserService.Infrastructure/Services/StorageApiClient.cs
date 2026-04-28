using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;
using UserService.Application.DTOs.Responses;
using UserService.Application.Services;

namespace UserService.Infrastructure.Services
{
    public class StorageApiClient : IStorageApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StorageApiClient> _logger;

        public StorageApiClient(HttpClient httpClient, ILogger<StorageApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> UploadProfileImageAsync(
            byte[] imageBytes,
            string fileName,
            string accessToken,
            CancellationToken cancellationToken)
        {
            using var form = new MultipartFormDataContent();

            var fileContent = new ByteArrayContent(imageBytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            form.Add(fileContent, "file", fileName);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7024/api/files/upload-avatar");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            request.Content = form;

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            var result = JsonSerializer.Deserialize<FileResponseDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result.url;
        }
    }
}
