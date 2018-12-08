using System;

namespace DevilDaggersWebsite.Models.API
{
	public class ApiFunctionAttribute : Attribute
	{
		public string Description { get; set; }
		public string ReturnType { get; set; }
		public bool IsDeprecated { get; set; }
		public string DeprecationMessage { get; set; }
	}
}