using DevilDaggersCore.Utils;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBotDdInfo
{
	public class CommandResult
	{
		public CommandResult(string? text, DiscordEmbed? embed = null)
		{
			Text = text;
			Embed = embed;
		}

		public string? Text { get; set; }
		public DiscordEmbed? Embed { get; set; }

		public static CommandResult GetIncorrectDate(string date)
		{
			DiscordEmbedBuilder builder = new()
			{
				Title = $"Invalid date \"{date}\"",
			};
			builder.AddField("Format", FormatUtils.DateFormat);
			return new(null, builder.Build());
		}

		public static CommandResult GetCustomResponse(string response)
		{
			DiscordEmbedBuilder builder = new()
			{
				Title = response,
				Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
				{
					Url = $"{NetworkUtils.BaseUrl}/Images/Icons/eye2.png",
				},
			};
			return new(null, builder.Build());
		}

		public static CommandResult GetIncorrectUsage(Type commandType, string commandName)
		{
			DiscordEmbedBuilder builder = new()
			{
				Title = "Incorrect command usage",
			};

			List<string> usages = new();
			foreach (MethodInfo method in commandType.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name.Contains("Execute", StringComparison.InvariantCulture)))
				usages.Add($".{commandName} {string.Join(' ', method.GetParameters().Select(p => $"<{p.Name}, {p.ParameterType.Name}>"))}");

			builder.AddField($"Valid usages of the `{commandType.Name}` are:", string.Join('\n', usages));

			return new(null, builder.Build());
		}

		public static CommandResult GetFromInvokedMethod(object? invokeResult)
		{
			if (invokeResult == null)
				return GetCustomResponse("Command invocation returned null.");

			if (invokeResult is CommandResult commandResult)
				return commandResult;

			if (invokeResult is Task<CommandResult> commandResultTask)
			{
				commandResultTask.Wait();
				return commandResultTask.Result;
			}

			return GetCustomResponse("Command invocation returned an incorrect type.");
		}
	}
}
