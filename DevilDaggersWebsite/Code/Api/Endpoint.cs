using System;

namespace DevilDaggersWebsite.Code.Api
{
	public class Endpoint
	{
		public Endpoint(string url, Type returnType, string newRouteToUse, EndpointParameter[] parameters, int[] statusCodes)
		{
			Url = url;
			ReturnType = returnType;
			NewRouteToUse = newRouteToUse;
			Parameters = parameters;
			StatusCodes = statusCodes;
		}

		public string Url { get; }

		public Type ReturnType { get; }

		/// <summary>
		/// Marks this endpoint as obsolete and specifies the new route that should be used.
		/// </summary>
		public string NewRouteToUse { get; }

		public EndpointParameter[] Parameters { get; }

		public int[] StatusCodes { get; }
	}
}