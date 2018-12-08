using Microsoft.AspNetCore.Html;
using System.Reflection;

namespace DevilDaggersWebsite.Models.API
{
	public class ApiFunction
	{
		public ApiFunctionAttribute Attribute { get; set; }
		public string Name { get; set; }
		public ParameterInfo[] Parameters { get; set; }

		public HtmlString FormattedDescription
		{
			get
			{
				char[] beginSeparators = new char[] { ' ', ',', '.', '(' };
				char[] endSeparators = new char[] { ' ', ',', '.', ')' };

				string formattedDescription = Attribute.Description;

				foreach (ParameterInfo pInfo in Parameters)
				{
					foreach (char begin in beginSeparators)
					{
						foreach (char end in endSeparators)
						{
							string parameter = $"{begin}{pInfo.Name}{end}";
							if (formattedDescription.Contains(parameter))
								formattedDescription = formattedDescription.Replace(parameter, $"<span style='color: {(pInfo.IsOptional ? "#ffff00" : "#ff0000")};'>{parameter}</span>");
						}
					}
				}
				return new HtmlString($"{(Attribute.IsDeprecated ? $"<span style='color: #ff8800;'>[DEPRECATED: {Attribute.DeprecationMessage}]</span> " : "")}{formattedDescription}");
			}
		}

		public ApiFunction(ApiFunctionAttribute attribute, string name, params ParameterInfo[] parameters)
		{
			Attribute = attribute;
			Name = name;
			Parameters = parameters;
		}
	}
}