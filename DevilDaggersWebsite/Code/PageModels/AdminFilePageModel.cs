using CoreBase3.Services;
using NetBase.Utils;
using System.IO;
using System.Text;

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

			FileContents = FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", this.fileName), Encoding.Default);
		}

		public void OnPost(string fileContents)
		{
			FileUtils.CreateText(Path.Combine(commonObjects.Env.WebRootPath, "user", fileName), fileContents, Encoding.UTF8);

			FileContents = fileContents;
		}
	}
}