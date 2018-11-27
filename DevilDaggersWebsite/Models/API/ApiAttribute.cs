using System;

namespace DevilDaggersWebsite.Models.API
{
	public class ApiAttribute : Attribute
	{
		public string ApiReturnType { get; set; }
	}
}