using DevilDaggersInfo.Web.Server.Domain.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevilDaggersInfo.Web.Server.Configuration;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
	private readonly IOptions<AuthenticationOptions> _authenticationOptions;

	public ConfigureJwtBearerOptions(IOptions<AuthenticationOptions> authenticationOptions)
	{
		_authenticationOptions = authenticationOptions;
	}

	public void Configure(string? name, JwtBearerOptions options)
	{
		if (name != JwtBearerDefaults.AuthenticationScheme)
			return;

		options.RequireHttpsMetadata = true;
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authenticationOptions.Value.JwtKey)),
			ValidateIssuer = false,
			ValidateAudience = false,
			ClockSkew = TimeSpan.Zero,
		};
	}

	public void Configure(JwtBearerOptions options)
	{
		Configure(string.Empty, options);
	}
}
