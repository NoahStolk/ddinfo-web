using CoreBase3.Services;
using DevilDaggersWebsite.Code.Users;
using Newtonsoft.Json;
using System.Collections.Generic;
using Io = System.IO;

namespace DevilDaggersWebsite.Code.PageModels
{
	public abstract class AdminFilePageModel<TData> : AdminPageModel
		where TData : AbstractUserData, new()
	{
		public string FileContents { get; set; }

		private readonly string fileName = new TData().FileName;

		private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
		{
			DefaultValueHandling = DefaultValueHandling.Ignore,
			NullValueHandling = NullValueHandling.Ignore
		};

		protected AdminFilePageModel(ICommonObjects commonObjects)
			: base(commonObjects)
		{
			FileContents = ReadJson();
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

			FileContents = ReadJson();
		}

		private string ReadJson()
			=> Io.File.ReadAllText(Io.Path.Combine(commonObjects.Env.WebRootPath, "user", $"{fileName}.json"));

		private void WriteJson(string fileContents)
		{
			List<TData> deserialized = JsonConvert.DeserializeObject<List<TData>>(fileContents, serializerSettings);
			string serialized = JsonConvert.SerializeObject(deserialized, Formatting.Indented, serializerSettings);

			Io.File.WriteAllText(Io.Path.Combine(commonObjects.Env.WebRootPath, "user", $"{fileName}.json"), serialized);
		}
	}
}