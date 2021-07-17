using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevilDaggersWebsite.Tests
{
	[TestClass]
	public class StructExtensionTests
	{
		[TestMethod]
		public void TestNullIfDefault()
		{
			Assert.IsNull(0.NullIfDefault());
			Assert.IsNotNull(1.NullIfDefault());

			Assert.IsNull(0.0.NullIfDefault());
			Assert.IsNotNull(1.0.NullIfDefault());

			Assert.IsNull(0UL.NullIfDefault());
			Assert.IsNotNull(1UL.NullIfDefault());
		}
	}
}
