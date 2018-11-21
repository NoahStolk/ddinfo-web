namespace DevilDaggersWebsite.Code
{
	public class ApiPage
	{
		public string Name { get; set; }
		public string[] Parameters { get; set; }

		public ApiPage(string name, params string[] parameters)
		{
			Name = name;
			Parameters = parameters;
		}
	}
}