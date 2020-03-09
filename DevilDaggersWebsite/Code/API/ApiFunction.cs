using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Code.Api
{
	public class ApiFunction
	{
		public ApiFunctionAttribute Attribute { get; }
		public string Name { get; }
		public ApiFunctionParameter[] Parameters { get; }
		public HtmlString FormattedDescription { get; }

		public ApiFunction(ApiFunctionAttribute attribute, string name, params ApiFunctionParameter[] parameters)
		{
			Attribute = attribute;
			Name = name;
			Parameters = parameters;

			char[] beginSeparators = new char[] { ' ', ',', '.', '(' };
			char[] endSeparators = new char[] { ' ', ',', '.', ')' };
			string formattedDescription = attribute.Description;
			foreach (ApiFunctionParameter param in Parameters)
			{
				foreach (char begin in beginSeparators)
				{
					foreach (char end in endSeparators)
					{
						string parameter = $"{begin}{param.Info.Name}{end}";
						if (formattedDescription.Contains(parameter))
							formattedDescription = formattedDescription.Replace(parameter, $"<span class='{(param.Info.IsOptional ? "api-parameter-optional" : "api-parameter")}'>{parameter}</span>");
					}
				}
			}
			FormattedDescription = new HtmlString($"{(attribute.IsDeprecated ? $"<span class='api-function-deprecated'>[DEPRECATED: {attribute.DeprecationMessage}]</span> " : "")}{formattedDescription}");
		}

		public HtmlString GetParameterDescriptions(Func<ApiFunctionParameter, bool> predicate)
		{
			IEnumerable<ApiFunctionParameter> parameters = Parameters.Where(predicate);
			if (parameters.Count() == 0)
				return RazorUtils.NAString;
			return new HtmlString(string.Join("<br />", parameters.Select(p => p.FormattedDescription)));
		}
	}
}