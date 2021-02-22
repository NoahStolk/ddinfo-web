using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DiscordBotDdInfo.Commands
{
	public class HelpCommand : AbstractCommand
	{
		public override string Name => "help";

		public CommandResult Execute()
		{
			DiscordEmbedBuilder builder = new() { Title = "List of commands", };

			foreach (AbstractCommand command in CommandHandler.Instance.CommandInstances)
			{
				List<string> usages = new();
				foreach (MethodInfo method in command.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name.Contains("Execute", StringComparison.InvariantCulture)))
					usages.Add($".{command.Name} {string.Join(' ', method.GetParameters().Select(p => $"<{p.Name}, {p.ParameterType.Name}>"))}");

				builder.AddField(command.GetType().Name, string.Join('\n', usages));
			}

			return new(null, builder.Build());
		}
	}
}
