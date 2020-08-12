using System;

namespace DevilDaggersWebsite.Code.Api
{
	public class Endpoint
	{
		public string Url { get; }
		public Type ReturnType { get; }
		public EndpointParameter[] Parameters { get; }
		public int[] StatusCodes { get; }

		public Endpoint(string url, Type returnType, EndpointParameter[] parameters, int[] statusCodes)
		{
			Url = url;
			ReturnType = returnType;
			Parameters = parameters;
			StatusCodes = statusCodes;
		}
	}
}