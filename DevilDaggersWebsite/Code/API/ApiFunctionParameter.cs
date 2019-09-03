using Microsoft.AspNetCore.Html;
using System;
using System.Reflection;

namespace DevilDaggersWebsite.Code.API
{
	public class ApiFunctionParameter
	{
		public ParameterInfo Info { get; }
		public HtmlString FormattedDescription { get; }

		private Type UnderlyingType => Nullable.GetUnderlyingType(Info.ParameterType);
		private bool IsNullable => UnderlyingType != null;
		private Type ActualType => IsNullable ? UnderlyingType : Info.ParameterType;

		public ApiFunctionParameter(ParameterInfo info)
		{
			Info = info;

			string typeSpan = $"<span class='api-parameter-type'>{ActualType.Name}</span>";
			typeSpan = IsNullable ? $"<span class='api-nullable'>Nullable&lt;{typeSpan}&gt;</span>" : typeSpan;
			
			string defaultValueSpan = $"<span class='api-parameter-default-value'>{GetDefaultValueString()}</span>";

			FormattedDescription = new HtmlString($"<span class='api-parameter{(info.IsOptional ? "-optional" : "")}'>{info.Name}</span> ({typeSpan}{(info.IsOptional ? $", {defaultValueSpan}" : "")})");
		}

		private string GetDefaultValueString()
		{
			if (ActualType.IsValueType)
			{
				if (IsNullable)
					return "<span class='api-null'>null</span>";
				if (Info.HasDefaultValue)
					return Info.DefaultValue.ToString();
				return Activator.CreateInstance(ActualType).ToString();
			}

			if (Info.HasDefaultValue && !string.IsNullOrEmpty((string)Info.DefaultValue))
				return Info.DefaultValue.ToString();
			return "<span class='api-null'>null</span>";
		}
	}
}