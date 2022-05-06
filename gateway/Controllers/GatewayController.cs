namespace WebApi.Controllers;

using Gateway.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common.Discovery;
using Gateway.Models.Responses;
using Steeltoe.Discovery;
using Newtonsoft.Json;
using Gateway.Models.Requests;

[Authorize]
[ApiController]
public class GatewayController : ControllerBase
{
    private readonly  IHttpClientFactory _clientFactory;

    public GatewayController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [Route("api/v1/messages")]
    [HttpGet]
    public async Task<IActionResult> GetMessage(int receiverId)
    {
        var client = _clientFactory.CreateClient("message-service");
        client.DefaultRequestHeaders.Add("user-id", Request.Headers["user-id"].ToString());
        var response = await client.GetStringAsync($"api/messages?receiverId={receiverId}");
        return Ok(JsonConvert.DeserializeObject<List<MessageResponse>>(response));
    }

    [Route("api/v1/messages")]
    [HttpPost]
    public async Task<IActionResult> AddMessage(MessageCreateRequest request)
    {
        var client = _clientFactory.CreateClient("message-service");
        client.DefaultRequestHeaders.Add("user-id", Request.Headers["user-id"].ToString());
        var response = await client.PostAsJsonAsync($"api/messages",request);
        return Ok();
    }

    [Route("api/v1/posts")]
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        var client = _clientFactory.CreateClient("posts-service");
        var response = await client.GetStringAsync("api/posts");
        return Ok(JsonConvert.DeserializeObject<List<PostsResponse>>(response));
    }

    [Route("api/v1/posts")]
    [HttpPost]
    public async Task<IActionResult> AddPost(PostCreateRequest request)
    {
        var client = _clientFactory.CreateClient("posts-service");
        client.DefaultRequestHeaders.Add("user-id", Request.Headers["user-id"].ToString());
        var response = await client.PostAsJsonAsync($"api/posts",request);
        return Ok();
    }

    [Route("api/v1/users/last-logon")]
    [HttpGet]
    public async Task<IActionResult> GetLastLogon()
    {
        var client = _clientFactory.CreateClient("logon-events");
        client.DefaultRequestHeaders.Add("user-id", Request.Headers["user-id"].ToString());
        var response = await client.GetStringAsync("api/v1/users/last-logon");
        return Ok(JsonConvert.DeserializeObject<LastLogonResponse>(response));
    }
}