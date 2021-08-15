using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers;

public class OidcConfigurationController : Controller
{
	private readonly ILogger<OidcConfigurationController> _logger;

	public OidcConfigurationController(IClientRequestParametersProvider clientRequestParametersProvider, ILogger<OidcConfigurationController> logger)
	{
		ClientRequestParametersProvider = clientRequestParametersProvider;
		_logger = logger;
	}

	public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

	[HttpGet("_configuration/{clientId}")]
	public IActionResult GetClientRequestParameters([FromRoute] string clientId)
	{
		IDictionary<string, string> parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
		return Ok(parameters);
	}
}
