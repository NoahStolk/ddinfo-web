using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Html;
using System.Reflection;

namespace DevilDaggersWebsite.Code.Api
{
	public class EndpointParameter
	{
		public EndpointParameter(ParameterInfo info)
		{
			Info = info;
			FormattedDescription = RazorUtils.GetFormattedParameter(info);
		}

		public ParameterInfo Info { get; }
		public HtmlString FormattedDescription { get; }
	}
}