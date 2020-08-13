using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Html;
using System.Reflection;

namespace DevilDaggersWebsite.Code.Api
{
	public class EndpointParameter
	{
		public ParameterInfo Info { get; }
		public HtmlString FormattedDescription { get; }

		public EndpointParameter(ParameterInfo info)
		{
			Info = info;
			FormattedDescription = RazorUtils.GetFormattedParameter(info);
		}
	}
}