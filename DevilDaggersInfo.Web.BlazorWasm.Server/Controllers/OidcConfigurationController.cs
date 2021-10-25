using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers;

public class OidcConfigurationController : Controller
{
	public OidcConfigurationController(IClientRequestParametersProvider clientRequestParametersProvider)
	{
		ClientRequestParametersProvider = clientRequestParametersProvider;
	}

	public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

	[HttpGet("_configuration/{clientId}")]
	public IActionResult GetClientRequestParameters([FromRoute] string clientId)
	{
		IDictionary<string, string> parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
		return Ok(parameters);
	}
}
