namespace DevilDaggersWebsite.Models.API
{
	public class ApiPage
	{
		public string Name { get; set; }
		public string ReturnType { get; set; }
		public string[] Parameters { get; set; }

		public ApiPage(string name, string returnType, params string[] parameters)
		{
			Name = name;
			ReturnType = returnType;
			Parameters = parameters;
		}
	}
}