namespace WebApi.Controllers;

using Gateway.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common.Discovery;
using Gateway.Models.Responses;
using Steeltoe.Discovery;

[Authorize]
[ApiController]
public class GatewayController : ControllerBase
{
    //private readonly IHttpClientFactory _httpClientFactory;
    private readonly  HttpClient _client;
    //DiscoveryHttpClientHandler _handler;

    public GatewayController(HttpClient client)
    {
        //_handler = new DiscoveryHttpClientHandler(client);
        //_httpClientFactory = httpClientFactory;
        _client = client;
    }

    [Route("api/messages")]
    [HttpGet]
    public async Task<IActionResult> AddMessage()
    {
        // var client = _httpClientFactory.CreateClient("message-service");
        _client.DefaultRequestHeaders.Add("user-id","1");
        return Ok(await _client.GetFromJsonAsync<MessageResponse>("http://message-service/api/messages"));
        // return Redirect("http://localhost:8081/api/messages");
    }

    // private HttpClient GetClient()
    // {
    //     var client = new HttpClient(_handler, false);
    //     return client;
    // }

}