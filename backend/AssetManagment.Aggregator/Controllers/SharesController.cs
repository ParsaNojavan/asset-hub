using AssetManagment.Aggregator.Models.Dtos;
using AssetManagment.Aggregator.Models.Dtos.Requests;
using AssetManagment.Aggregator.Models.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;

namespace AssetManagment.Aggregator.Controllers
{
    [Route("api/shares")]
    [Authorize]
    [ApiController]
    public class SharesController : ControllerBase
    {

        private readonly string shareUrl = "https://localhost:7165";
        const string storageUrl = "https://localhost:7024";
        const string userUrl = "https://localhost:7119";

        private readonly IHttpClientFactory _httpClientFactory;

        public SharesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("inbox")]
        public async Task<IActionResult> GetInboxAsset([FromQuery] PageRequestDto pageRequest)
        {
            var client = _httpClientFactory.CreateClient("default");
            var sharedAssetsResponse = await client
                .GetAsync($"{shareUrl}/api/shares/shared-with-me?Page={pageRequest.Page}&PageSize={pageRequest.PageSize}");
            sharedAssetsResponse.EnsureSuccessStatusCode();

            var sharedAssets = await sharedAssetsResponse.Content.ReadFromJsonAsync<ShareDetails>();

            var tasks = sharedAssets.Items.Select(async item =>
            {
                var fileTask = client.GetFromJsonAsync<AssetDto>($"{storageUrl}/api/files/{item.ResourceId}");
                var userTask = client.GetFromJsonAsync<UserDto>($"{userUrl}/api/user/{item.OwnerUserId}");

                await Task.WhenAll(fileTask, userTask);

                return new AssetDetailsDto
                {
                    Asset = fileTask.Result,
                    User = userTask.Result
                };
            });

            var details = (await Task.WhenAll(tasks)).ToList();


            return Ok(new { Items = details, total = sharedAssets.TotalCount });
        }

        [HttpGet("outbox")]
        public async Task<IActionResult> GetOutboxAsset([FromQuery] PageRequestDto pageRequest)
        {
            var client = _httpClientFactory.CreateClient("default");

            var sharedAssetsResponse = await client
                .GetAsync($"{shareUrl}/api/shares/my-shares?Page={pageRequest.Page}&PageSize={pageRequest.PageSize}");

            sharedAssetsResponse.EnsureSuccessStatusCode();

            var sharedAssets = await sharedAssetsResponse.Content.ReadFromJsonAsync<ShareDetails>();

            var json = await sharedAssetsResponse.Content.ReadAsStringAsync();


            var tasks = sharedAssets.Items.Select(async item =>
            {
                var asset = await client.GetFromJsonAsync<AssetDto>($"{storageUrl}/api/files/{item.ResourceId}");

                return new AssetWithShareDto
                {
                    ShareId = item.Id,
                    Asset = asset
                };
            });

            var details = (await Task.WhenAll(tasks)).ToList();

            return Ok(new {Items=details,total = sharedAssets.TotalCount});
        }


        [HttpGet("{shareId:guid}")]
        public async Task<IActionResult> GetSharedAssetDetails(Guid shareId)
        {
            var client = _httpClientFactory.CreateClient("default");

            var sharedAssetResponse = await client
                .GetAsync($"{shareUrl}/api/shares/{shareId}");

            sharedAssetResponse.EnsureSuccessStatusCode();

            var sharedAsset = await sharedAssetResponse.Content.ReadFromJsonAsync<SharedAssetDto>();

            if (sharedAsset == null)
                return NotFound();

            var userTask = client.GetFromJsonAsync<UserDto>($"{userUrl}/api/user/{sharedAsset.OwnerUserId}");
            var assetTask = client.GetFromJsonAsync<AssetDto>($"{storageUrl}/api/files/{sharedAsset.ResourceId}");

            var receiverTasks = sharedAsset.Users
                .Select(u => client.GetFromJsonAsync<UserDto>($"{userUrl}/api/user/{u.UserId}"))
                .ToList();

            await Task.WhenAll(receiverTasks.Concat(new Task[] { userTask, assetTask }));

            var receivers = receiverTasks
                .Select(t => t.Result)
                .Where(x => x != null)
                .ToList();

            var details = new SharedAssetDetails
            {
                Asset = new AssetDetailsDto
                {
                    Asset = assetTask.Result,
                    User = userTask.Result
                },
                Recivers = receivers
            };

            return Ok(details);
        }


    }
}
