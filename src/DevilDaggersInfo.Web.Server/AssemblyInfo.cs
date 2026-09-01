using System.Runtime.CompilerServices;

// The integration tests boot the real host through WebApplicationFactory<Program>, which needs the compiler-generated
// entry point type, and filter the application's own hosted services out of the service collection by type. They also
// cover internal helpers such as RewriteRulesUtils.
[assembly: InternalsVisibleTo("DevilDaggersInfo.Web.Server.IntegrationTest")]
