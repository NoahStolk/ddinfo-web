using System;
using System.Reflection;

namespace DevilDaggersWebsite.Code.Api
{
	public class Endpoint
	{
		public string Url { get; }
		public Type ReturnType { get; }
		public ParameterInfo[] Parameters { get; }
		public int[] StatusCodes { get; }

		public Endpoint(string url, Type returnType, ParameterInfo[] parameters, int[] statusCodes)
		{
			Url = url;
			ReturnType = returnType;
			Parameters = parameters;
			StatusCodes = statusCodes;
		}
	}
}