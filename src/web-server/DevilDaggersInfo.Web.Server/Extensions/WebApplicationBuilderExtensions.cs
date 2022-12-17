using DevilDaggersInfo.Web.Server.NSwag;
using NJsonSchema;

namespace DevilDaggersInfo.Web.Server.Extensions;

public static class WebApplicationBuilderExtensions
{
	public static void AddValidatedOptions<TOptions>(this WebApplicationBuilder builder, string configSection)
		where TOptions : class
	{
		builder.Services.AddOptions<TOptions>()
			.Bind(builder.Configuration.GetRequiredSection(configSection), o => o.ErrorOnUnknownConfiguration = true)
			.ValidateOnStart()
			.ValidateDataAnnotations();
	}

	public static void AddSwaggerDocument(this WebApplicationBuilder builder, string apiNamespace, string description)
	{
		builder.Services.AddSwaggerDocument(config =>
		{
			config.PostProcess = document =>
			{
				document.Info.Title = $"DevilDaggers.info API ({apiNamespace.ToUpper()})";
				document.Info.Description = description;
				document.Info.Contact = new()
				{
					Name = "Noah Stolk", Url = "//noahstolk.com/",
				};
			};
			config.DocumentName = apiNamespace.ToUpper();
			config.OperationProcessors.Insert(0, new ApiOperationProcessor(apiNamespace));
			config.SchemaType = SchemaType.OpenApi3;
			config.GenerateEnumMappingDescription = true;
		});
	}
}
