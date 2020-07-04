using CoreBase3.Services;
using Newtonsoft.Json;
using Io = System.IO;

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

			ReadJson();
		}

		public void OnPost(string fileContents)
		{
			try
			{
				if (JsonConvert.DeserializeObject(fileContents) == null)
					return;
			}
			catch
			{
				return;
			}

			WriteJson(fileContents);

			ReadJson();
		}

		private void ReadJson()
			=> FileContents = Io.File.ReadAllText(Io.Path.Combine(commonObjects.Env.WebRootPath, "user", $"{fileName}.json"));

		private void WriteJson(string fileContents)
			=> Io.File.WriteAllText(Io.Path.Combine(commonObjects.Env.WebRootPath, "user", $"{fileName}.json"), fileContents);
	}
}