using AssetManagment.Aggregator.Models.Dtos;
using AssetManagment.Aggregator.Models.Dtos.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagment.Aggregator.Controllers
{
    [Route("api/assets")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        const string storageUrl = "https://localhost:7024";
        const string userUrl = "https://localhost:7119";

        private readonly IHttpClientFactory _httpClientFactory;

        public AssetController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{assetId:guid}")]
        public async Task<IActionResult> GetAssetDetails([FromRoute]Guid assetId)
        {
            var client = _httpClientFactory.CreateClient("default");
            var storageResponse = await client.GetAsync($"{storageUrl}/api/files/{assetId}");
            storageResponse.EnsureSuccessStatusCode();

            var asset = await storageResponse.Content.ReadFromJsonAsync<AssetDto>();

            var userResponse = await client.GetAsync($"{userUrl}/api/user/{asset.UserId}");
            userResponse.EnsureSuccessStatusCode();

            var user = await userResponse.Content.ReadFromJsonAsync<UserDto>();

            var details = new AssetDetailsDto
            {
                Asset = asset,
                User = user
            };

            return Ok(details);
        }
    }
}
