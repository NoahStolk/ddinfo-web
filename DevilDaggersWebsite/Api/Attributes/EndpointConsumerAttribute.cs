using System;

namespace DevilDaggersWebsite.Api.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class EndpointConsumerAttribute : Attribute
	{
		public EndpointConsumerAttribute(EndpointConsumers endpointConsumers)
		{
			EndpointConsumers = endpointConsumers;
		}

		public EndpointConsumers EndpointConsumers { get; init; }
	}
}
