using DevilDaggersWebsite.Code.Tasks.Scheduling;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class TasksModel : PageModel
    {
		public Dictionary<string, DateTime> LastUpdatedDictionary { get; private set; } = new Dictionary<string, DateTime>();

        public void OnGet()
        {
			Assembly asm = AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.FullName.Contains("DevilDaggersWebsite"))
				.FirstOrDefault();

			foreach (Type type in asm.GetTypes())
				if (typeof(IScheduledTask).IsAssignableFrom(type) && type.Namespace.Contains("Tasks"))
					foreach (PropertyInfo pInfo in type.GetProperties())
						if (pInfo.Name == "LastUpdated")	
							LastUpdatedDictionary[type.Name] = (DateTime)pInfo.GetValue(null, null);
		}
    }
}