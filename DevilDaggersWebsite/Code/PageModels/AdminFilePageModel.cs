using CoreBase3.Services;
using System.IO;

namespace DevilDaggersWebsite.Code.PageModels
{
	public abstract class AdminFilePageModel : AdminPageModel
	{
		private readonly string fileName;

		public string FileContents { get; set; }

		protected AdminFilePageModel(ICommonObjects commonObjects, string fileName)
			: base(commonObjects)
		{
			this.fileName = fileName;

			FileContents = System.IO.File.ReadAllText(Path.Combine(commonObjects.Env.WebRootPath, "user", $"{this.fileName}.json"));
		}

		public void OnPost(string fileContents)
		{
			System.IO.File.WriteAllText(Path.Combine(commonObjects.Env.WebRootPath, "user", $"{fileName}.json"), fileContents);

			FileContents = fileContents;
		}
	}
}